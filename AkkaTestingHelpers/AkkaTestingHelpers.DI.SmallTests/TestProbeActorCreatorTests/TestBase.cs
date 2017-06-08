﻿using System;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    internal class TestBase : TestKit
    {
        protected Func<Type> GenerateType;
        protected Type ActorType;

        [SetUp]
        public void SetUp()
        {
            GenerateType = TestUtils.RandomTypeGenerator();
            ActorType = GenerateType();
        }

        [TearDown]
        public void TearDown()
        {
            GenerateType = null;
            ActorType = null;
        }

        protected TestProbeActorCreator CreateTestProbeActorFactory() => new TestProbeActorCreator();
    }
}