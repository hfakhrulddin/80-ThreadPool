// SimplePool.cs
// Simple thread pool example
using System;
using System.Threading;

namespace ThreadPoolG
{
    public class SimplePool
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Thread Pool Sample:");
            Console.WriteLine("Thread will start soon !!!");


            bool W2K = false;
            int MaxCount = 10;  // Allow a total of 10 threads in the pool
                                // Mark the event as unsignaled.
            ManualResetEvent eventX = new ManualResetEvent(false);
            Console.WriteLine("Queuing {0} items to Thread Pool", MaxCount);
            Alpha oAlpha = new Alpha(MaxCount);  // Create the work items.
                                                 // Make sure the work items have a reference to the signaling event.
            oAlpha.eventX = eventX;
            Console.WriteLine("Queue to Thread Pool 0");
            try
            {
                // Queue the work items, which has the added effect of checking
                // which OS is running.
                ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta),
                   new SomeState(0));
                W2K = true;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("These API's may fail when called on a non-Windows 2000 system.");
                W2K = false;
            }
            if (W2K)  // If running on an OS which supports the ThreadPool methods.
            {
                for (int iItem = 1; iItem < MaxCount; iItem++)
                {
                    // Queue the work items:
                    Console.WriteLine("Queue to Thread Pool {0}", iItem);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(oAlpha.Beta), new SomeState(iItem));
                }
                Console.WriteLine("Waiting for Thread Pool to drain");
                // The call to exventX.WaitOne sets the event to wait until
                // eventX.Set() occurs.
                // (See oAlpha.Beta).
                // Wait until event is fired, meaning eventX.Set() was called:
                eventX.WaitOne(Timeout.Infinite, true);
                // The WaitOne won't return until the event has been signaled.
                Console.WriteLine("Thread Pool has been drained (Event fired)");
                Console.WriteLine();
                Console.WriteLine("Load across threads");
                foreach (object o in oAlpha.HashCount.Keys)
                    Console.WriteLine("{0} {1}", o, oAlpha.HashCount[o]);
                Console.WriteLine();
                Console.WriteLine("Press Enter To Exit!!!");
                Console.ReadLine();
            }
            return 0;
        }
    }
}
