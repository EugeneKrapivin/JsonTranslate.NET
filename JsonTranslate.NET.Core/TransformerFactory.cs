using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Transformers.Core.Abstractions;

namespace Transformers.Core
{
    public sealed class TransformerFactory : ITransformerFactory
    {
        private static readonly ConcurrentDictionary<string, (TypeInfo type, TransformerAttribute attr)> _transformers =
            new();

        static TransformerFactory()
        {
            var types =
                from type in Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes
                let attr = type.GetCustomAttribute<TransformerAttribute>()
                where type.ImplementedInterfaces.Any(x => x == typeof(IJTokenTransformer))
                    && attr is not null
                select (type, attr);

            foreach (var type in types)
            {
                RegisterTransformer(type);
            }
        }

        public static void RegisterTransformer<T>()
        {
            var (type, attr) = ValidateType(typeof(T).GetTypeInfo());

            RegisterInternal(type, attr);
        }

        public static void RegisterTransformer((TypeInfo type, TransformerAttribute attr) _)
        {
            var (type, attr) = _;

            ValidateType(type);

            RegisterInternal(type, attr);
        }

        private static (TypeInfo type, TransformerAttribute attr) ValidateType(TypeInfo type)
        {
            if (!ImplementsInterface(type.GetTypeInfo()))
                throw new ArgumentException($"{type.Name} should implement {nameof(IJTokenTransformer)} and ");

            var attr = GetTransformerAttribute(type);
            if (attr == null)
                throw new ArgumentException(
                    $"type {type.Name} must be decorated with an attribute of type {nameof(TransformerAttribute)}");

            return (type, attr);
        }

        private static bool ImplementsInterface(TypeInfo type)
        {
            return typeof(IJTokenTransformer).IsAssignableFrom(type);
        }

        private static TransformerAttribute GetTransformerAttribute(Type type)
            => type.GetCustomAttribute<TransformerAttribute>();

        private static void RegisterInternal(TypeInfo type, TransformerAttribute decorator)
        {
            _transformers.TryAdd(decorator.Name, (type, decorator));
        }

        public IJTokenTransformer GetTransformer(string name, JObject conf = null)
        {
            if (!_transformers.TryGetValue(name, out var value))
            {
                throw new Exception("bummer mate, nothing found");
            }

            var (type, decorator) = value;

            if (decorator.RequiresConfig)
            {
                if (conf == null)
                    throw new ArgumentNullException(nameof(conf),
                        $"transformer of type {decorator.Name} requires config");

                return Activator.CreateInstance(type, conf) as IJTokenTransformer;
            }

            return Activator.CreateInstance(type) as IJTokenTransformer;
        }

        public TypeInfo RemoveTransformer(string name) =>
            _transformers.TryRemove(name, out var pair) 
                ? pair.type 
                : null;

        public ITransformerFactory AddTransformer<T>() where T : IJTokenTransformer
        {
            RegisterTransformer<T>();

            return this;
        }
    }
}