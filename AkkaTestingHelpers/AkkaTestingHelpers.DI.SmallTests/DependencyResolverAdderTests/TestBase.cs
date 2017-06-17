﻿using Akka.Actor;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    internal class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        protected DependencyResolverAdder CreateDependencyResolverAdder() => new DependencyResolverAdder();

        protected class DummyActor : ReceiveActor { }
    }
}