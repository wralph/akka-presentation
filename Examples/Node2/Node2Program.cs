using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node2
{
    class Node2Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    log-config-on-start = off
    stdout-loglevel = DEBUG
    loglevel = INFO
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
        
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
		    applied-adapters = []
		    transport-protocol = tcp
		    port = 8080
		    hostname = localhost
        }
    }
}
");
            using (ActorSystem.Create("system2", config))
            {
                Console.ReadLine();
            }
        }
    }
}
