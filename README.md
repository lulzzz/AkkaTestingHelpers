# AkkaTestingHelpers
This NuGet packages offers helpers for both unit testing and integration testing.

## Unit testing
The `UnitTestFramework` in the package allows you to test an Actor in full isolation. The framework creates the actor to be tested (referred to by the framework as SUT: System Under Test) with a TestProbe as its parent. It also replaces its children with TestProbes.

It can be used to test the following scenarios:
- That an actor sends the correct messages to its children
- That an actor sends the correct messages to its parent
- That an actor processes replies from its children correctly
- What names an actor gives to its children
- What types an actor creates as its children
- What supervisor strategies an actor creates its children with

The framework replaces children with `TestProbe` objects by using `Akka.DI`. This means you must create child actors like this for the framework to function correctly:

``` csharp
var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-1");
```

## Examples
To see some examples on how to use the `UnitTestFramework`. See the [examples](AkkaTestingHelpers.MediumTests/UnitTestFrameworkTests/Examples) folder.

### Usage guide
#### Initiating the framework
To create an instance of the `UnitTestFramework` you must first create an `UnitTestFrameworkSettings` object. This is done using the Empty property of the settings object:

``` csharp
var settings = UnitTestFrameworkSettings.Empty;
```

The settings object allows you to register message handlers against it. A message handler is a method that is ran when a particular type of child actor receives a particular type of message. The return value of the method is sent back to the actor that sent the message.

The following example registers a handler that is invoked whenever any children of the type `ExampleActor` receive a message of the type `int`. The handler doubles the `int` and send it back to the actor who sent it the original message.

``` csharp
var settings = UnitTestFrameworkSettings
    .Empty
    .RegisterHandler<ExampleActor, int>(i => i * 2));
```

You can then create the framework object from the settings object. Using the `CreateFramework` method. When creating the framework you must specify the type of actor you wish to test along with a `TestKit` instance. If the actor you wish to test (the SUT actor) does not have a default constructor you must give a `Prop` object to create the actor. If the SUT actor creates children in its constructor you must specify how many children it creates. The `CreateFramework` method only returns once all the children have been created. We'll see why this is done later.

The example below creates a framework with `ParentActor` as the SUT actor and waits for the `ParentActor`'s constructor to create 2 child actors. Note that `this` in the example is an instance of `TestKit`.

``` csharp
var framework = UnitTestFrameworkSettings
    .Empty
    .RegisterHandler<ExampleActor, int>(i => i * 2))
    .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(), 2));
```

#### Using the framework
> Note: some of these examples use the `FluentAssertions` NuGet package.

Once you have a framework you send messages to your SUT actor by using the sut property. The following example sends a message of the type `string` to the sut actor.
``` csharp
framework.Sut.Tell("hello world");
```

If the SUT actor sends messages to its parent/supervisor you can test this like so:
``` csharp
framework.Sut.Tell("hello world");
framework.Supervisor.Expect("hello world");
```

If the SUT actor creates a child actor of the type `ExampleActor` with the name `"child-1"` then you can test this like so:
``` csharp
framework.ResolvedType("child-1").Should().Be<ExampleActor>()
```

> Note: this is the reason the `CreateFramework` waits for children to be resolved before returning. If it did not then the child may not had been resolved when we did our assertion

If the SUT actor sends `int` messages to a child actor with the name `"child-2"` then you can test this like so:
``` csharp
framework.Sut.Tell(5);
framework.ResolvedTestProbe("child-2").ExpectMsg(5);
```

If the SUT actor creates a child with a `SupervisorStrategy` of the type `OneForOneStrategy` with a retry limit of 5, then you can test this like so:
``` csharp
framework.Sut.Tell(5);
framework.ResolvedSupervisorStrategy("child-1")
    .As<OneForOneStrategy>().MaxNumberOfRetries
    .Should().Be(5);
```

> If a child actor is created using a `Props` object that specifies a `SupervisorStrategy` then that will be returned (E.g. `Context.DI.Props<ChildActor>().WithSupervisorStrategy(new AllForOneStrategy(exception => Directive.Escalate))`). If it is not, the private SupervisorStrategy property of the SUT is returned (E.g. `Context.DI.Props<ChildActor>()`)

If the SUT actor creates `2` new children when it receives a `string` message, you can wait for those children to be created like so:

``` csharp
sut.TellMessageAndWaitForChildren("hello", 2);
```

This means you can then go on use the `ResolvedType`, `ResolvedTestProbe` and `ResolvedSupervisorStrategy` methods safely knowing the new actors have been created.

## Integration testing
