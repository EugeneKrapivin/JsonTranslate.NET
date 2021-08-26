using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonTranslate.NET.Core;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    public static class TransformersGenericSources
    {
        public static IEnumerable<Type> BoundTransformers
        {
            get
            {
                var types = Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes;
                var list = types
                    .Where(type => CustomAttributeExtensions.GetCustomAttribute<TransformerAttribute>((MemberInfo) type) is not null)
                    .Where(x => x.ImplementedInterfaces.Any(iface => iface == typeof(IJTokenTransformer)))
                    .Where(x => x.BaseType != typeof(ValueProvidingTransformer))
                    .ToList();

                return list;
            }
        }

        public static IEnumerable<Type> ValueProvidingTransformers
        {
            get
            {
                var types = Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes;
                var list = types
                    .Where(type => type.GetCustomAttribute<TransformerAttribute>() is not null)
                    .Where(x => x.ImplementedInterfaces.Any(iface => iface == typeof(IJTokenTransformer)))
                    .Where(x => x.BaseType == typeof(ValueProvidingTransformer))
                    .Where(x => x.DeclaredConstructors.Any(ctor => ctor.GetParameters().Length == 1 
                            && ctor.GetParameters().Single().ParameterType == typeof(JObject)))
                    .ToList();

                return list;
            }
        }

        public static IEnumerable<Type> MultiBoundTransformers
        {
            get
            {
                var types = Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes;
                var list = types
                    .Where(type => type.GetCustomAttribute<TransformerAttribute>() is not null)
                    .Where(x => x.ImplementedInterfaces.Any(iface => iface == typeof(IJTokenTransformer)))
                    .Where(x => x.BaseType == typeof(MultiBoundTransformer))
                    .ToList();

                return list;
            }
        }

        public static IEnumerable<Type> SinglyBoundTransformers
        {
            get
            {
                var types = Assembly.GetAssembly(typeof(TransformerFactory)).DefinedTypes;
                var list = types
                    .Where(type => type.GetCustomAttribute<TransformerAttribute>() is not null)
                    .Where(x => x.ImplementedInterfaces.Any(iface => iface == typeof(IJTokenTransformer)))
                    .Where(x => x.BaseType == typeof(SinglyBoundTransformer))
                    .ToList();

                return list;
            }
        }
    }
}