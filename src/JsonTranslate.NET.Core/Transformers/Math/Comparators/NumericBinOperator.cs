using System.Collections.Generic;

using JsonTranslate.NET.Core.Abstractions.Transformers;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    public abstract class NumericBinOperator : AbstractBinOperator
    {
        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.Numeric;

        public override IEnumerable<JTokenType> OutputTypes => JTokenTypeConstants.Numeric;

        protected override JToken Operate(JToken loper, JToken roper)
        {
            loper.EnsureNumeric();
            roper.EnsureNumeric();

            return OperateInternal(loper, roper);
        }

        protected abstract JToken OperateInternal(JToken loper, JToken roper);
    }
}