using NSubstitute;
using Shouldly;
using Xunit;

namespace Fixera.Tests
{
    public class DefaultFakeProviderTests
    {
        public DefaultFakeProviderTests()
        {
            _fakeFactory = Substitute.For<IFakeFactory>();

            _sut = new DefaultFakeProvider(_fakeFactory);
        }

        private readonly IFakeFactory _fakeFactory;
        private readonly DefaultFakeProvider _sut;

        [Fact]
        public void GetTransient_returns_new_fake_each_time()
        {
            var expected1 = Substitute.For<ISomeInterface>();
            var expected2 = Substitute.For<ISomeInterface>();

            _fakeFactory.Create<ISomeInterface>().Returns(expected1, expected2);

            var actual1 = _sut.GetTransient<ISomeInterface>();
            var actual2 = _sut.GetTransient<ISomeInterface>();

            actual1.ShouldNotBeSameAs(actual2);
            actual1.ShouldBeSameAs(expected1);
            actual2.ShouldBeSameAs(expected2);
        }

        [Fact]
        public void GetSingleton_returns_same_fake_every_time()
        {
            var expected = Substitute.For<ISomeInterface>();

            _fakeFactory.Create<ISomeInterface>().Returns(expected);

            var actual1 = _sut.GetSingleton<ISomeInterface>();
            var actual2 = _sut.GetSingleton<ISomeInterface>();

            actual1.ShouldBeSameAs(expected);
            actual1.ShouldBeSameAs(actual2);
            _fakeFactory.Received(1).Create<ISomeInterface>();
        }

        [Fact]
        public void TryAdd_sets_the_singleton()
        {
            var expected = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(expected).ShouldBeTrue();
            _sut.GetSingleton<ISomeInterface>().ShouldBeSameAs(expected);
            _fakeFactory.DidNotReceive().Create<ISomeInterface>();
        }

        [Fact]
        public void TryAdd_does_not_overwrite_auto_created()
        {
            var autoCreated = Substitute.For<ISomeInterface>();
            _fakeFactory.Create<ISomeInterface>().Returns(autoCreated);

            _sut.GetSingleton<ISomeInterface>().ShouldBeSameAs(autoCreated);

            var later = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(later).ShouldBeFalse();
            _sut.GetSingleton<ISomeInterface>().ShouldBeSameAs(autoCreated);
            _fakeFactory.Received(1).Create<ISomeInterface>();
        }

        [Fact]
        public void TryAdd_does_not_overwrite_previously_added()
        {
            var previous = Substitute.For<ISomeInterface>();
            _sut.TryAddSingleton(previous);

            var later = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(later).ShouldBeFalse();
            _sut.GetSingleton<ISomeInterface>().ShouldBeSameAs(previous);
            _fakeFactory.Received(0).Create<ISomeInterface>();
        }
    }
}
