namespace Fixera
{
    public interface IFakeProvider
    {
        T GetTransient<T>() where T : class;

        T GetSingleton<T>() where T : class;

        bool TryAddSingleton<T>(T instance) where T : class;

        void Clear();
    }
}
