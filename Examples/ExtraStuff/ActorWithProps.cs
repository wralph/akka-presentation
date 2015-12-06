using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraStuff
{
    public class DemoActor : ReceiveActor
    {
        public static Props Props(int magicNumber)
        {
            return Props.Create(() => new DemoActor(magicNumber));
        }

        private int magicNumber;

        public DemoActor(int magicNumber)
        {
            this.magicNumber = magicNumber;
            ...some receive declarations here
            //Receive<string>(str => ..);
        }
    }

    // creation
    //system.ActorOf(DemoActor.Props(42), "demo");
}
