namespace Fixera
{
    public interface IFakeFactory
    {
        T Create<T>() where T : class;
    }
}
