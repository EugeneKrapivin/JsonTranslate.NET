﻿using System;
using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Collections
{
    [Transformer("obj")]
    public class ObjectTransformer : MultiBoundTransformer
    {
        public override IEnumerable<JTokenType> InputTypes => new[] { JTokenType.Property };
        
        public override IEnumerable<JTokenType> OutputTypes => new[] { JTokenType.Object };

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);
            
            if (source is PropertyTransformer)
            {
                _sources.Add(source);
            }
            else
            {
                throw new NotSupportedException(
                    $"Transformer of type `{nameof(ObjectTransformer)}` only supports bindings of type `{nameof(PropertyTransformer)}`");
            }

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            var properties = _sources.Select(x => x.Transform(root, ctx)).ToArray();

            return new JObject(properties);
        }
    }
}