using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Transformers.String.Reducers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    public static class TransformerCreationHelpers
    {
        public static IJTokenTransformer GetBindingFor<TTarget>() where TTarget : IJTokenTransformer =>
            typeof(TTarget) == typeof(ObjectTransformer)
                ? new KeyedTransformer()
                : GetTransformer<TTarget>();

        public static TTarget GetTransformer<TTarget>() =>
            typeof(TTarget).Name switch
            {
                nameof(ValueProvidingTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget), new JObject {["path"] = "$"}),
                nameof(UnitTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget), new JObject {["value"] = "$"}),
                nameof(LookupTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget), JObject.FromObject(new LookupTransformer.LookupConfig())),
                nameof(StringJoinAggregator) => (TTarget)Activator.CreateInstance(typeof(TTarget), JObject.FromObject(new StringJoinAggregator.StringJoinAggregatorConfig())),
                _ => Activator.CreateInstance<TTarget>()
            };
    }
}