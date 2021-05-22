using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Fixera.Tests
{
    [TestFixture]
    public class DefaultFakeProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeFactory = Substitute.For<IFakeFactory>();
            _sut = new DefaultFakeProvider(_fakeFactory);
        }

        private IFakeFactory _fakeFactory = default!;
        private DefaultFakeProvider _sut = default!;

        [Test]
        public void GetTransient_returns_new_fake_each_time()
        {
            var expected1 = Substitute.For<ISomeInterface>();
            var expected2 = Substitute.For<ISomeInterface>();

            _fakeFactory.Create<ISomeInterface>().Returns(expected1, expected2);

            var actual1 = _sut.GetTransient<ISomeInterface>();
            var actual2 = _sut.GetTransient<ISomeInterface>();

            actual1.Should().NotBeSameAs(actual2);
            actual1.Should().BeSameAs(expected1);
            actual2.Should().BeSameAs(expected2);
        }

        [Test]
        public void GetSingleton_returns_same_fake_every_time()
        {
            var expected = Substitute.For<ISomeInterface>();

            _fakeFactory.Create<ISomeInterface>().Returns(expected);

            var actual1 = _sut.GetSingleton<ISomeInterface>();
            var actual2 = _sut.GetSingleton<ISomeInterface>();

            actual1.Should().BeSameAs(expected);
            actual1.Should().BeSameAs(actual2);
            _fakeFactory.Received(1).Create<ISomeInterface>();
        }

        [Test]
        public void TryAdd_sets_the_singleton()
        {
            var expected = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(expected).Should().BeTrue();
            _sut.GetSingleton<ISomeInterface>().Should().BeSameAs(expected);
            _fakeFactory.DidNotReceive().Create<ISomeInterface>();
        }

        [Test]
        public void TryAdd_does_not_overwrite_auto_created()
        {
            var autoCreated = Substitute.For<ISomeInterface>();
            _fakeFactory.Create<ISomeInterface>().Returns(autoCreated);

            _sut.GetSingleton<ISomeInterface>().Should().BeSameAs(autoCreated);

            var later = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(later).Should().BeFalse();
            _sut.GetSingleton<ISomeInterface>().Should().BeSameAs(autoCreated);
            _fakeFactory.Received(1).Create<ISomeInterface>();
        }

        [Test]
        public void TryAdd_does_not_overwrite_previously_added()
        {
            var previous = Substitute.For<ISomeInterface>();
            _sut.TryAddSingleton(previous);

            var later = Substitute.For<ISomeInterface>();

            _sut.TryAddSingleton(later).Should().BeFalse();
            _sut.GetSingleton<ISomeInterface>().Should().BeSameAs(previous);
            _fakeFactory.Received(0).Create<ISomeInterface>();
        }
    }
}
