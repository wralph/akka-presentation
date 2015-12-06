using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HelloWorldDDC
{
    class Program
    {
        static void Main(string[] args)
        {
             using (var system = ActorSystem.Create("mySystem"))
            {
                var helloActor = system.ActorOf<HelloActor>();
                helloActor.Tell(new HelloMessage("Hallo dotnet developer conference"));
                Console.ReadLine();
            }
        }
    }

    class HelloMessage
    {
        public HelloMessage(string msg)
        {
            this.Message = msg;
        }
        public string Message { get; private set; }
    }

    class HelloActor : ReceiveActor
    {
        public HelloActor()
        {
            Receive<HelloMessage>(x => Console.WriteLine(x.Message));
        }
    }
}
