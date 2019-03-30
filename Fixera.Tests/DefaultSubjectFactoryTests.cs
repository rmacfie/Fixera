using NSubstitute;
using Shouldly;
using Xunit;

namespace Fixera.Tests
{
    public class DefaultSubjectFactoryTests
    {
        public DefaultSubjectFactoryTests()
        {
            _fakeProvider = Substitute.For<IFakeProvider>();
        }

        private readonly IFakeProvider _fakeProvider;

        [Fact]
        public void It_creates_type_with_dependencies_and_defaults()
        {
            var dependency1 = Substitute.For<IDependency1>();
            var dependency2 = Substitute.For<IDependency2>();

            _fakeProvider.GetSingleton<IDependency1>().Returns(dependency1);
            _fakeProvider.GetSingleton<IDependency2>().Returns(dependency2);

            var sut = new DefaultSubjectFactory<SomeClassWithDependencies>(_fakeProvider);

            var created = sut.Create();

            created.ShouldNotBeNull();
            created.Dependency1.ShouldBeSameAs(dependency1);
            created.Dependency2.ShouldBeSameAs(dependency2);
            created.Dependency3.ShouldBe(SomeClassWithDependencies.DefaultDependency3);
            created.Dependency4.ShouldBe(SomeClassWithDependencies.DefaultDependency4);
        }

        [Fact]
        public void It_creates_type_without_dependencies()
        {
            var sut = new DefaultSubjectFactory<SomeClassWithoutDependencies>(_fakeProvider);

            var created = sut.Create();

            created.ShouldNotBeNull();
        }
    }
}
