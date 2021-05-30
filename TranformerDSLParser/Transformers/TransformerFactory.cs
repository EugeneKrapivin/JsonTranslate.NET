using System;
using System.Collections.Concurrent;
using System.Reflection;
using Newtonsoft.Json.Linq;
using TranformerDSLParser.Core;

namespace TranformerDSLParser.Transformers
{
    public class TransformerFactory
    {
        private static readonly ConcurrentDictionary<string, (Type type, TransformerAttribute attr)> _transformers = new();

        public TransformerFactory RegisterTransformer<T>()
        {
            var type = typeof(T);
            if (!type.IsAssignableTo(typeof(IJTokenTransformer)))
                throw new ArgumentException($"{type.Name} should implement {nameof(IJTokenTransformer)} and ");

            var decorator = GetDecorator<T>();

            if (decorator == null) throw new ArgumentException($"type {typeof(T).Name} must be decorated with an attribute of type {nameof(TransformerAttribute)}");

            _transformers.TryAdd(decorator.Name, (typeof(T), decorator));

            return this;
        }

        public static void SelfRegisterTransformer<T>()
        {
            var type = typeof(T);
            if (!type.IsAssignableTo(typeof(IJTokenTransformer)))
                throw new ArgumentException($"{type.Name} should implement {nameof(IJTokenTransformer)} and ");

            var decorator = GetDecorator<T>();

            if (decorator == null) throw new ArgumentException($"type {typeof(T).Name} must be decorated with an attribute of type {nameof(TransformerAttribute)}");

            _transformers.TryAdd(decorator.Name, (typeof(T), decorator));
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
                if (conf == null) throw new ArgumentNullException(nameof(conf), $"transformer of type {decorator.Name} requires config");

                return Activator.CreateInstance(type, conf) as IJTokenTransformer;
            }

            return Activator.CreateInstance(type) as IJTokenTransformer;
        }

        private static TransformerAttribute GetDecorator<T>()
            => typeof(T).GetCustomAttribute<TransformerAttribute>();
    }
}