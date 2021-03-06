﻿using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface IConcreteDependencyResolverAdder
    {
        void Add(
            TestKitBase testKit,
            ImmutableDictionary<Type, Func<ActorBase>> factories);
    }
}