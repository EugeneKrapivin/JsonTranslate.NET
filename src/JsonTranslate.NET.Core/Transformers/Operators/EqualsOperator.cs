using System.Collections.Generic;
using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Abstractions.Transformers;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    [Transformer("eq")]
    public class EqualsOperator : AbstractBinOperator
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Primitive;

        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Primitive;
        
        protected override JToken Operate(JToken loper, JToken roper) => JToken.DeepEquals(loper, roper);
    }

    [Transformer("neq")]
    public class NotEqualsOperator : EqualsOperator
    {
        protected override JToken Operate(JToken loper, JToken roper) => !JToken.DeepEquals(loper, roper);
    }

    public abstract class NumericBinOperator : AbstractBinOperator
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.Numeric;

        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Numeric;

        protected override JToken Operate(JToken loper, JToken roper)
        {
            loper.EnsureNumeric();
            roper.EnsureNumeric();

            return OperateInternal(loper, roper);
        }

        protected abstract JToken OperateInternal(JToken loper, JToken roper);
    }

    [Transformer("gt")]
    public class GreaterThanOperator : NumericBinOperator
    {
        protected override JToken OperateInternal(JToken loper, JToken roper) => loper.Value<decimal>() > roper.Value<decimal>();
    }

    [Transformer("gte")]
    public class GreaterThanEqualsOperator : NumericBinOperator
    {
        protected override JToken OperateInternal(JToken loper, JToken roper) => loper.Value<decimal>() >= roper.Value<decimal>();
    }

    [Transformer("lt")]
    public class LessThanOperator : NumericBinOperator
    {
        protected override JToken OperateInternal(JToken loper, JToken roper) => loper.Value<decimal>() < roper.Value<decimal>();
    }

    [Transformer("lte")]
    public class LessThanEqualsOperator : NumericBinOperator
    {
        protected override JToken OperateInternal(JToken loper, JToken roper) => loper.Value<decimal>() <= roper.Value<decimal>();
    }
}