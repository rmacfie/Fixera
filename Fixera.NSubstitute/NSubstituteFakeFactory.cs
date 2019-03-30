using System;
using NSubstitute;

namespace Fixera
{
    public class NSubstituteFakeFactory : IFakeFactory
    {
        public T Create<T>() where T : class
        {
            return Substitute.For<T>();
        }

        public object Create(Type type)
        {
            return Substitute.For(new[] {type}, new object[0]);
        }
    }
}
