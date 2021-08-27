using System.Collections.Generic;
using System.Linq;
using JsonTranslate.NET.Core.Abstractions.Exceptions;
using Newtonsoft.Json.Linq;

namespace JsonTranslate.NET.Core.Abstractions.Transformers
{
    public abstract class ValueProvidingTransformer : TransformerBase
    {
        public override IEnumerable<JTokenType> InputTypes => JTokenTypeConstants.None;
        
        public override IEnumerable<JTokenType> OutputTypes => JTokenTypeConstants.Any;
        
        public override IEnumerable<IJTokenTransformer> Sources { get; } = Enumerable.Empty<IJTokenTransformer>();
        
        public override IJTokenTransformer Bind(IJTokenTransformer source) => 
            throw new ValueProvidersCannotBeBoundException(nameof(ValueProvidingTransformer));
    }
}