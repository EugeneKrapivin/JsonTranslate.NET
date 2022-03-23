using System;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using JsonTranslate.NET.Core.Transformers;
using JsonTranslate.NET.Core.Transformers.Collections;
using JsonTranslate.NET.Core.Transformers.Date;
using JsonTranslate.NET.Core.Transformers.String.Reducers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Transformer.UnitTests.Abstractions
{
    public static class TransformerCreationHelpers
    {
        public static IJTokenTransformer GetBindingFor<TTarget>() where TTarget : IJTokenTransformer =>
            typeof(TTarget) == typeof(ObjectTransformer)
                ? new PropertyTransformer()
                : GetTransformer<TTarget>();

        public static TTarget GetTransformer<TTarget>()
            where TTarget : IJTokenTransformer =>
            typeof(TTarget).Name switch
            {
                nameof(ValueOfTransformer) =>
                    (TTarget) Activator.CreateInstance(typeof(TTarget), new JObject {["path"] = "$"}),
                nameof(UnitTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget),
                    new JObject {["value"] = "$"}),
                nameof(LookupTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget),
                    JObject.FromObject(new LookupTransformer.LookupConfig())),
                nameof(StringJoinTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget),
                    JObject.FromObject(new StringJoinTransformer.StringJoinTransformerConfig())),
                nameof(DateToDateTransformer) => (TTarget) Activator.CreateInstance(typeof(TTarget),
                    JObject.FromObject(new DateToDateTransformer.DateFormatTransformerConfig())),
                nameof(ToIsoDateTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget),
                    JObject.FromObject(new ToIsoDateTransformer.ToIsoDateTransformerConfig())),
                nameof(ToUnixTimestampTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget),
                    JObject.FromObject(new ToUnixTimestampTransformer.ToUnixTimestampTransformerConfig())),
                _ => Activator.CreateInstance<TTarget>()
            };

        public static (TTarget, JObject) GetTransformerWithConfig<TTarget>()
            where TTarget : IJTokenTransformer
        {
            JObject config = GetConfig<TTarget>();
            return (GetTransformerWithConfig<TTarget>(config), config);
        }

        public static JObject GetConfig<TTarget>()
            where TTarget : IJTokenTransformer =>
            typeof(TTarget).Name switch
            {
                nameof(ValueOfTransformer) => new JObject { ["path"] = "$" },
                nameof(UnitTransformer) =>  new JObject { ["value"] = "$" },
                nameof(LookupTransformer) =>  JObject.FromObject(new LookupTransformer.LookupConfig()),
                nameof(StringJoinTransformer) =>  JObject.FromObject(new StringJoinTransformer.StringJoinTransformerConfig()),
                var x => throw new Exception($"Unknown configurable transformer of type {x}")
            };

        public static TTarget GetTransformerWithConfig<TTarget>(JObject config)
            where TTarget : IJTokenTransformer =>
            typeof(TTarget).Name switch
            {
                nameof(ValueOfTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget), config),
                nameof(UnitTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget), config),
                nameof(LookupTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget), config),
                nameof(StringJoinTransformer) => (TTarget)Activator.CreateInstance(typeof(TTarget), config),
                var x => throw new Exception($"Unknown configurable transformer of type {x}")
            };
    }
}