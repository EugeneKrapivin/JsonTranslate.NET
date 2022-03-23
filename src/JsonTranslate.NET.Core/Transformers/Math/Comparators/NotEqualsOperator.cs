using JsonTranslate.NET.Core.Abstractions;

using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Transformers.Operators
{
    [Transformer("neq")]
    public class NotEqualsOperator : EqualsOperator
    {
        protected override JToken Operate(JToken loper, JToken roper) => !JToken.DeepEquals(loper, roper);
    }
}