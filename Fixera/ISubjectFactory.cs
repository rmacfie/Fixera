namespace Fixera
{
    public interface ISubjectFactory<out T>
        where T : class
    {
        T Create();
    }
}
