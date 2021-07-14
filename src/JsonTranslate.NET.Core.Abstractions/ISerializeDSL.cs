namespace JsonTranslate.NET.Core.Abstractions
{
    public interface ISerializeDSL
    {
        string ToString(Instruction instructions);
        
        Instruction Parse(string dsl);
    }
}
