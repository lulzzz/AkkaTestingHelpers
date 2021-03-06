﻿using System;
using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedTestProbe : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_ResolvedTestProbeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.ResolvedTestProbe(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_ResolvedTestProbe_ReturnsCorrectProbe()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            TestProbe result = sut.ResolvedTestProbe(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedTestProbe);
        }
    }
}