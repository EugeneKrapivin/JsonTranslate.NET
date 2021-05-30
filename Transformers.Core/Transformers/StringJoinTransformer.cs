﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Transformers.Core.Abstractions;

namespace Transformers.Core.Transformers
{
    [Transformer(name: "str_join", requiresConfig: true)]
    public class StringJoinTransformer : IJTokenTransformer
    {
        static StringJoinTransformer()
        {
            TransformerFactory.RegisterTransformer<StringToStringLookupTransformer>();
        }

        public string SourceType => "string";

        public string TargetType => "string";

        private readonly Config _config;

        private readonly List<IJTokenTransformer> _sources = new();

        public StringJoinTransformer(JObject conf)
        {
            if (conf == null) throw new ArgumentNullException($"{nameof(ValueOfTransformer)} requires configuration");

            _config = this.GetConfig<Config>(conf);
        }

        public IJTokenTransformer Bind(params IJTokenTransformer[] sources)
        {
            // todo: validate all sources have target types string
            _sources.AddRange(sources);

            return this;
        }

        public JToken Transform(JToken root)
        {
            // TODO: validate at least 1 source
            var values = _sources.Select(x => Extensions.Value<string>(x.Transform(root)));

            return string.Join(_config.Separator, values);
        }

        private class Config
        {
            public string Separator { get; set; }
        }
    }
}