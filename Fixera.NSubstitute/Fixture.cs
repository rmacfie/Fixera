namespace Fixera
{
    public abstract class Fixture : FixtureBase
    {
        protected Fixture() : base(new NSubstituteFakeFactory())
        {
        }
    }
}
