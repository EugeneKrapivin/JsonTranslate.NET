using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.String.Operators
{
    [Transformer(name: "firstindexof", requiresConfig: false)]
    public class FirstIndexOfReducer : StringOperator
    {
        public override IEnumerable<JTokenType> OutputTypes => new[] { JTokenType.Integer };

        protected override JToken Operate(string loper, string roper, TransformationContext ctx = null)
            => loper.IndexOf(roper, StringComparison.InvariantCulture);
    }
}