﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedActors
{
    public class SomeActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            Console.WriteLine("{0} got {1}", Self.Path.ToStringWithAddress(), message);
        }
    }
}
