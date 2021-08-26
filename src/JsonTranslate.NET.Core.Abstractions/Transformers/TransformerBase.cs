using System;
using JsonTranslate.NET.Core.Abstractions.Exceptions;

using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Linq;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public abstract class TransformerBase : IJTokenTransformer
    {
        public abstract IJTokenTransformer Bind(IJTokenTransformer source);

        public virtual IEnumerable<JTokenType> SupportedTypes => Enumerable.Empty<JTokenType>();

        public virtual IEnumerable<JTokenType> SupportedResults => Enumerable.Empty<JTokenType>();

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

    public abstract class SinglyBoundTransformer : TransformerBase
    {
        protected IJTokenTransformer _source;
        
        private List<IJTokenTransformer> _sources = new(1); // ugly hack :(

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            if (_source != null)
            {
                throw new CanHaveOnlyOneBindingException(GetType().Name);
            }

            EnsureSource(source);
            
            _source = source ?? throw new CannotBindToNullException();

            _sources.Add(_source);

            EnsureNoCycles();

            return this;
        }

        public override JToken Transform(JToken root, TransformationContext ctx = null)
        {
            EnsureSource(_source);

            return TransformSingle(root, ctx);
        }

        protected abstract JToken TransformSingle(JToken root, TransformationContext ctx = null);
    }

    public abstract class MultiBoundTransformer : TransformerBase
    {
        protected readonly List<IJTokenTransformer> _sources = new();

        public override IEnumerable<IJTokenTransformer> Sources => _sources;

        public override IJTokenTransformer Bind(IJTokenTransformer source)
        {
            EnsureSource(source);

            _sources.Add(source ?? throw new CannotBindToNullException());

            EnsureNoCycles();

            return this;
        }
    }


    public abstract class ValueProvidingTransformer : TransformerBase
    {
        public override IEnumerable<JTokenType> SupportedTypes => JTokenTypeConstants.None;
        
        public override IEnumerable<JTokenType> SupportedResults => JTokenTypeConstants.Any;
        
        public override IEnumerable<IJTokenTransformer> Sources { get; } = Enumerable.Empty<IJTokenTransformer>();
        
        public override IJTokenTransformer Bind(IJTokenTransformer source) => 
            throw new ValueProvidersCannotBeBoundException(nameof(ValueProvidingTransformer));
    }
}
