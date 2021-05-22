using System;
using System.Collections.Concurrent;

namespace Fixera
{
    public class DefaultFakeProvider : IFakeProvider
    {
        private readonly IFakeFactory _factory;
        private readonly ConcurrentDictionary<Type, object> _singletons = new();

        public DefaultFakeProvider(IFakeFactory factory)
        {
            _factory = factory;
        }

        public TFake GetTransient<TFake>()
            where TFake : class
        {
            return _factory.Create<TFake>();
        }

        public TFake GetSingleton<TFake>()
            where TFake : class
        {
            return (TFake) _singletons.GetOrAdd(typeof(TFake), _ => _factory.Create<TFake>());
        }

        public bool TryAddSingleton<TFake>(TFake instance)
            where TFake : class
        {
            return _singletons.TryAdd(typeof(TFake), instance);
        }

        public void Clear()
        {
            _singletons.Clear();
        }
    }
}
