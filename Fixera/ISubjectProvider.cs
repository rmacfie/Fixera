namespace Fixera
{
    public interface ISubjectProvider<T>
        where T : class
    {
        T Get();

        bool TryAdd(T instance);

        void Clear();
    }
}
