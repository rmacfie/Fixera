namespace Fixera.Tests
{
    public interface ISomeInterface
    {
    }

    public class SomeClassWithoutDependencies : ISomeInterface
    {
    }

    public class SomeClassWithDependencies : ISomeInterface
    {
        public const string DefaultDependency3 = "foo";
        public const int DefaultDependency4 = 999;

        public readonly IDependency1 Dependency1;
        public readonly IDependency2 Dependency2;
        public readonly string Dependency3;
        public readonly int Dependency4;

        public SomeClassWithDependencies(
            IDependency1 dependency1,
            IDependency2 dependency2,
            string dependency3 = DefaultDependency3,
            int dependency4 = DefaultDependency4
        )
        {
            Dependency1 = dependency1;
            Dependency2 = dependency2;
            Dependency3 = dependency3;
            Dependency4 = dependency4;
        }
    }

    public interface IDependency1
    {
    }

    public interface IDependency2
    {
    }
}
