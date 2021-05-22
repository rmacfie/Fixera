using System;
using System.Diagnostics.CodeAnalysis;

namespace Fixera
{
    public abstract class FixtureBase<TSubject> : FixtureBase where TSubject : class
    {
        public readonly ISubjectProvider<TSubject> SubjectProvider;

        protected FixtureBase(IFakeFactory fakeFactory)
            : this(new DefaultFakeProvider(fakeFactory))
        {
        }

        protected FixtureBase(IFakeProvider fakeProvider)
            : this(fakeProvider, new DefaultSubjectFactory<TSubject>(fakeProvider))
        {
        }

        protected FixtureBase(IFakeProvider fakeProvider, ISubjectFactory<TSubject> subjectFactory)
            : this(fakeProvider, new DefaultSubjectProvider<TSubject>(subjectFactory))
        {
        }

        protected FixtureBase(IFakeProvider fakeProvider, ISubjectProvider<TSubject> subjectProvider)
            : base(fakeProvider)
        {
            SubjectProvider = subjectProvider;
        }

        public TSubject SUT
        {
            get => GetSubject();
            set => SetSubject(value);
        }

        private TSubject GetSubject()
        {
            return SubjectProvider.Get();
        }

        private void SetSubject(TSubject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (!SubjectProvider.TryAdd(subject))
                throw new InvalidOperationException("The SUT (Subject Under Test) is already created and can not be changed");
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class FixtureBase
    {
        public readonly IFakeProvider FakeProvider;

        protected FixtureBase(IFakeFactory fakeFactory)
            : this(new DefaultFakeProvider(fakeFactory))
        {
        }

        protected FixtureBase(IFakeProvider fakeProvider)
        {
            FakeProvider = fakeProvider;
        }

        public T an<T>() where T : class
        {
            return FakeProvider.GetTransient<T>();
        }

        public T the<T>() where T : class
        {
            return FakeProvider.GetSingleton<T>();
        }

        public T the<T>(T instance) where T : class
        {
            if (!FakeProvider.TryAddSingleton(instance))
                throw new InvalidOperationException($"The instance for {typeof(T).FullName} is already created and can not be changed");

            return instance;
        }
    }
}
