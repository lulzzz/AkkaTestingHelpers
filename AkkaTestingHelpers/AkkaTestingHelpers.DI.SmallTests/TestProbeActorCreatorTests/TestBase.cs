﻿using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    internal class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        protected TestProbeActorCreator CreateTestProbeActorFactory() => new TestProbeActorCreator();
    }
}