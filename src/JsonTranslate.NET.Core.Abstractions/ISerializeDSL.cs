namespace JsonTranslate.NET.Core.Abstractions
{
    public interface ISerializeDsl
    {
        string Serialize(Instruction instructions);
        
        Instruction Deserialize(string dsl);
    }
}
