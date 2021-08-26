using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Operators
{

    [Transformer(name: "lastindexof", requiresConfig: false)]
    public class LastIndexOfReducer : StringOperator
    {
        public override IEnumerable<JTokenType> SupportedResults => new[] { JTokenType.Integer };

        protected override JToken Operate(string loper, string roper, TransformationContext ctx = null) 
            => loper.LastIndexOf(roper, StringComparison.InvariantCulture);
    }
}