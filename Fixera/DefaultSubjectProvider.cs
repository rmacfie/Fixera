using System.Collections.Concurrent;

namespace Fixera
{
    public class DefaultSubjectProvider<T> : ISubjectProvider<T>
        where T : class
    {
        private const int Key = 0;

        private readonly ISubjectFactory<T> _factory;
        private readonly ConcurrentDictionary<int, T> _subjects = new();

        public DefaultSubjectProvider(ISubjectFactory<T> factory)
        {
            _factory = factory;
        }

        public T Get()
        {
            return _subjects.GetOrAdd(Key, _ => _factory.Create());
        }

        public bool TryAdd(T instance)
        {
            return _subjects.TryAdd(Key, instance);
        }

        public void Clear()
        {
            _subjects.Clear();
        }
    }
}
