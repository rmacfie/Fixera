# fixera

Simple automocking for .NET unit tests


## Installation

```bash
dotnet add package Fixera.NSubstitute
```

This will install `Fixera` and the NSubstitute adapter `Fixera.NSubstitute`.


## Usage

```csharp
public class MyTest : Fixture<MyService>
{
    [Fact]
    public void It_gets_the_thing()
    {
        var theThing = new Thing(123);
        the<ISomeRepository>().FindThing(123).Returns(theThing);

        var actual = SUT.GetTheThing(123);

        actual.ShouldBeSameAs(theThing);
    }
}
```


## API

Each mock library adapter implements test base classes `Fixture` and `Fixture<TSubject>`.

The `Fixture` class exposes the following methods to subclasses:

- `an<T>()` creates a fake/mock of type `T`.
- `the<T>()` creates a fake/mock of type `T` once, and then returns the same instance for every subsequent call to `the<T>()`.
- `the<T>(T instance)` set up your own instance.

The `Fixture<TSubject>` exposes the same methods, plus:

- `TSubject SUT`

which is a property that automatically instantiates the subject-under-test, using `the<T>`
to fulfill any constructor dependencies.


## License

The source code and packages are licensed under the MIT license.
