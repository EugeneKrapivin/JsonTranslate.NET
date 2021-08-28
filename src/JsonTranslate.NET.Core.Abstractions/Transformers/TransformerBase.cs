using JsonTranslate.NET.Core.Abstractions.Exceptions;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Linq;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public abstract class TransformerBase : IJTokenTransformer
    {
        public abstract IJTokenTransformer Bind(IJTokenTransformer source);

        public virtual JObject Config => null;

        public virtual IEnumerable<JTokenType> InputTypes => Enumerable.Empty<JTokenType>();

        public virtual IEnumerable<JTokenType> OutputTypes => Enumerable.Empty<JTokenType>();

        public virtual IEnumerable<IJTokenTransformer> Sources => Enumerable.Empty<IJTokenTransformer>();

        public abstract JToken Transform(JToken root, TransformationContext ctx = null);

        public TR Accept<TR>(IVisitor<IJTokenTransformer, TR> visitor)
        {
            return visitor.Visit(this);
        }

        protected virtual void EnsureSource(IJTokenTransformer source)
        {
            if (ReferenceEquals(this, source))
                throw new CannotBindToSelfException();

            if (source is null)
                throw new CannotBindToNullException();
        }

        protected bool CheckTreeCycles()
        {
            var visited = new HashSet<IJTokenTransformer>();
            var stack = new Stack<IJTokenTransformer>();

            stack.Push(this);

            while (stack.Any())
            {
                var transformer = stack.Pop();

                if (!visited.Add(transformer))
                {
                    return true;
                }

                foreach (var adjacent in transformer.Sources)
                {
                    stack.Push(adjacent);
                }
            }

            return false;
        }

        protected void EnsureNoCycles()
        {
            var hasCycle = CheckTreeCycles();

            if (hasCycle)
            {
                throw new BindingCreateGraphCycleException(GetType().Name);
            }
        }
    }
}
