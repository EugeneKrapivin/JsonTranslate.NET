using JsonTranslate.NET.Core.Abstractions;
using JsonTranslate.NET.Core.Transformers.Operators;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace JsonTranslate.NET.Core.Transformers.Math.Operators
{
    [Transformer("div")]
    public class DivOperator : AbstractBinOperator
    {
        protected override JToken Operate(JToken loper, JToken roper)
        {
            var r = roper.Value<decimal>();
            if (r == 0)
            {
                throw new DivideByZeroException();
            }
            var l = loper.Value<decimal>();

            return l / r;
        }
    }
}
