using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Fixera.Tests
{
    [TestFixture]
    public class DefaultSubjectFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeProvider = Substitute.For<IFakeProvider>();
        }

        private IFakeProvider _fakeProvider = default!;

        [Test]
        public void It_creates_type_with_dependencies_and_defaults()
        {
            var dependency1 = Substitute.For<IDependency1>();
            var dependency2 = Substitute.For<IDependency2>();

            _fakeProvider.GetSingleton<IDependency1>().Returns(dependency1);
            _fakeProvider.GetSingleton<IDependency2>().Returns(dependency2);

            var sut = new DefaultSubjectFactory<SomeClassWithDependencies>(_fakeProvider);

            var created = sut.Create();

            created.Should().NotBeNull();
            created.Dependency1.Should().BeSameAs(dependency1);
            created.Dependency2.Should().BeSameAs(dependency2);
            created.Dependency3.Should().Be(SomeClassWithDependencies.DefaultDependency3);
            created.Dependency4.Should().Be(SomeClassWithDependencies.DefaultDependency4);
        }

        [Test]
        public void It_creates_type_without_dependencies()
        {
            var sut = new DefaultSubjectFactory<SomeClassWithoutDependencies>(_fakeProvider);

            var created = sut.Create();

            created.Should().NotBeNull();
        }
    }
}
