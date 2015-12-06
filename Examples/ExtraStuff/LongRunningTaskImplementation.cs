using Akka.Actor;
using Akka.Dispatch.SysMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtraStuff
{
    /*
     * https://petabridge.com/blog/top-7-akkadotnet-stumbling-blocks/ 
     */
    public class FooActor : ReceiveActor,
                        IWithUnboundedStash
    {

        private Task _runningTask;
        private CancellationTokenSource _cancel;

        public IStash Stash { get; set; }

        public FooActor()
        {
            _cancel = new CancellationTokenSource();
            Ready();
        }

        private void Ready(){
            Receive<Start>(s =>
            {
                var self = Self; // closure
                _runningTask = Task.Run(() =>
                {
                    // ... work
                }, _cancel.Token).ContinueWith(x =>
                {                    
                    if (x.IsCanceled || x.IsFaulted)
                        return new Failed();
                    return new Finished();
                }, TaskContinuationOptions.ExecuteSynchronously)
                .PipeTo(self);

                // switch behavior
                Become(Working);
            });
        }

        private void Working()
        {
            Receive<Cancel>(cancel =>
            {
                _cancel.Cancel(); // cancel work
                BecomeReady();
            });
            Receive<Failed>(f => BecomeReady());
            Receive<Finished>(f => BecomeReady());
            ReceiveAny(o => Stash.Stash());
        }

        private void BecomeReady()
        {
            _cancel = new CancellationTokenSource();
            Stash.UnstashAll();
            Become(Ready);
        }
    }

    class Cancel
    {
    }

    class Finished
    {
    }

    class Start
    {
    }

    class Failed
    {
    }
}
