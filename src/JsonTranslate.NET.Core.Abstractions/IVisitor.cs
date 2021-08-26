namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IVisitor<in T, out TR>
    {
        TR Visit(T target);
    }
}