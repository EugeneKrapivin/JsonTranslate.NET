using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core
{
    [ExcludeFromCodeCoverage]
    public sealed class TransformerFactory : ITransformerFactory
    {
        private readonly ConcurrentDictionary<string, (TypeInfo type, TransformerAttribute attr)> _transformers =
            new();

        public TransformerFactory(bool scan = true)
        {
            if (!scan) return;

            var asmTypes = Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes;

            RegisterTypes(asmTypes);
        }

        public TransformerFactory(Type assemblyContainingType)
        {
            if (assemblyContainingType == null) throw new ArgumentNullException(nameof(assemblyContainingType));

            var asmTypes = Assembly.GetAssembly(assemblyContainingType).DefinedTypes;

            RegisterTypes(asmTypes);
        }

        public TransformerFactory(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var asmTypes = assembly.DefinedTypes;

            RegisterTypes(asmTypes);
        }

        public TransformerFactory(params TypeInfo[] transformerTypes)
        {
            RegisterTypes(transformerTypes);
        }

        public IJTokenTransformer GetTransformerInstance(string name, JObject conf = null)
        {
            if (!_transformers.TryGetValue(name, out var value))
            {
                throw new Exception();
            }

            var (type, _) = value;

            return conf == null 
                ? Activator.CreateInstance(type) as IJTokenTransformer
                : Activator.CreateInstance(type, conf) as IJTokenTransformer;
        }

        public TypeInfo RemoveTransformer(string name)
        {
            return _transformers.TryRemove(name, out var pair)
                ? pair.type
                : throw new Exception();
        }

        public ITransformerFactory AddTransformer<T>() where T : IJTokenTransformer
        {
            RegisterTransformer<T>();

            return this;
        }

        public IJTokenTransformer BuildTransformationTree(Instruction instruction) =>
            (instruction as IAccepting<Instruction>)
            .Accept(new TransformationBuildingVisitor(this));

        private void RegisterTypes(IEnumerable<TypeInfo> types)
        {
            foreach (var (type, attr) in FilterTransformerTypes(types))
            {
                RegisterInternal(type, attr);
            }
        }

        private void RegisterTransformer<T>()
        {
            var (type, attr) = ValidateType(typeof(T).GetTypeInfo());

            RegisterInternal(type, attr);
        }

        private void RegisterInternal(TypeInfo type, TransformerAttribute decorator)
        {
            _transformers.TryAdd(decorator.Name, (type, decorator));
        }

        private static IEnumerable<(TypeInfo type, TransformerAttribute attr)> FilterTransformerTypes(IEnumerable<TypeInfo> types) =>
            from type in types
            let attr = type.GetCustomAttribute<TransformerAttribute>()
            where type.ImplementedInterfaces.Any(x => x == typeof(IJTokenTransformer))
                  && attr is not null
            select (type, attr);

        private static (TypeInfo type, TransformerAttribute attr) ValidateType(TypeInfo type)
        {
            var attr = GetTransformerAttribute(type);
            if (attr == null)
            {
                throw new Exception();
            }

            return (type, attr);
        }

        private static TransformerAttribute GetTransformerAttribute(Type type)
        {
            return type.GetCustomAttribute<TransformerAttribute>();
        }

        private class TransformationBuildingVisitor : IVisitor<Instruction, IJTokenTransformer>
        {
            private readonly ITransformerFactory _factory;

            public TransformationBuildingVisitor(ITransformerFactory factory)
            {
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public IJTokenTransformer Visit(Instruction target)
            {
                var (name, config, bindings) = target;

                var root = _factory.GetTransformerInstance(name, config);

                foreach (IAccepting<Instruction> binding in bindings ?? Enumerable.Empty<Instruction>())
                {
                    root.Bind(binding.Accept(this));
                }

                return root;
            }
        }
    }
}