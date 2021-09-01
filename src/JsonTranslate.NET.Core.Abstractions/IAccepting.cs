namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IAccepting<out T>
    {
        TR Accept<TR>(IVisitor<T, TR> visitor);
    }
}