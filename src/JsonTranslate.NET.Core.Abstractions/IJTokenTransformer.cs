﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IJTokenTransformer : IAccepting<IJTokenTransformer>
    {
        public JObject Config { get; }

        public IEnumerable<JTokenType> InputTypes { get; }

        public IEnumerable<JTokenType> OutputTypes { get; }

        public IEnumerable<IJTokenTransformer> Sources { get; }

        JToken Transform(JToken root, TransformationContext ctx = null);

        IJTokenTransformer Bind(IJTokenTransformer source);
    }
}