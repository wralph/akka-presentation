using Akka.Actor;
using Akka.Configuration;
using SharedMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class ChatClientProgram
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
		    applied-adapters = []
		    transport-protocol = tcp
		    port = 0
		    hostname = localhost
        }
    }
}
");

            using (var system = ActorSystem.Create("MyClient", config))
            {
                var chatClient = system.ActorOf(Props.Create<ChatClientActor>());
                chatClient.Tell(new ConnectRequest()
                {
                    Username = "dotnet developer conference member",
                });

                while (true)
                {
                    var input = Console.ReadLine();
                    if (input.StartsWith("/"))
                    {
                        var parts = input.Split(' ');
                        var cmd = parts[0].ToLowerInvariant();
                        var rest = string.Join(" ", parts.Skip(1));

                        if (cmd == "/nick")
                        {
                            chatClient.Tell(new NickRequest
                            {
                                NewUsername = rest
                            });
                        }
                    }
                    else
                    {
                        chatClient.Tell(new SayRequest()
                        {
                            Text = input,
                        });
                    }
                }
            }
        }
    }

    class ChatClientActor : TypedActor,
        IHandle<ConnectRequest>,
        IHandle<ConnectResponse>,
        IHandle<NickRequest>,
        IHandle<NickResponse>,
        IHandle<SayRequest>,
        IHandle<SayResponse>, ILogReceive
    {
        private string _nick = "dotnet developer conference member";
        private readonly ActorSelection _server = Context.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ChatServer");

        public void Handle(ConnectResponse message)
        {
            Console.WriteLine("Connected!");
            Console.WriteLine(message.Message);
        }

        public void Handle(NickRequest message)
        {
            message.OldUsername = this._nick;
            Console.WriteLine("Changing nick to {0}", message.NewUsername);
            this._nick = message.NewUsername;
            _server.Tell(message);
        }

        public void Handle(NickResponse message)
        {
            Console.WriteLine("{0} is now known as {1}", message.OldUsername, message.NewUsername);
        }

        public void Handle(SayResponse message)
        {
            Console.WriteLine("{0}: {1}", message.Username, message.Text);
        }

        public void Handle(ConnectRequest message)
        {
            Console.WriteLine("Connecting....");
            _server.Tell(message);
        }

        public void Handle(SayRequest message)
        {
            message.Username = this._nick;
            _server.Tell(message);
        }
    }
}
