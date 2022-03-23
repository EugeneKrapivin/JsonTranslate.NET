using System.Collections.Generic;

using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    [Transformer("eq")]
    public class EqualsOperator : AbstractBinOperator
    {
        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.Primitive;

        public override IEnumerable<JTokenType> OutputTypes => JTokenTypeConstants.Primitive;
        
        protected override JToken Operate(JToken loper, JToken roper) => JToken.DeepEquals(loper, roper);
    }
}