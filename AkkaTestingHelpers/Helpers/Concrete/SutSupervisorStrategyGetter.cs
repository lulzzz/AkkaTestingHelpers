﻿using System.Reflection;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class SutSupervisorStrategyGetter : ISutSupervisorStrategyGetter
    {
        private static readonly MethodInfo SupervisorStrategyMethod = 
            typeof(ActorBase)
                .GetTypeInfo()
                .GetMethod(
                    "SupervisorStrategy", 
                    BindingFlags.Instance | BindingFlags.NonPublic);

        public SupervisorStrategy Get(ActorBase actor) => 
            (SupervisorStrategy)SupervisorStrategyMethod.Invoke(actor, new object[0]);
    }
}