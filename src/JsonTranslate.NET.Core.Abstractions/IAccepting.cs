namespace JsonTranslate.NET.Core.Abstractions
{
    public interface IAccepting<out T>
    {
        public TR Accept<TR>(IVisitor<T, TR> visitor)
        {
            return visitor.Visit((T)this);
        }
    }
}