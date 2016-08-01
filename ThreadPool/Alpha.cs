using System;
using System.Collections;
using System.Threading;

namespace ThreadPoolG
{
    public class Alpha
    {
            public Hashtable HashCount;
            public ManualResetEvent eventX;
            public static int iCount = 0;
            public static int iMaxCount = 0;

            public Alpha(int MaxCount)
            {
                HashCount = new Hashtable(MaxCount);
                iMaxCount = MaxCount;
            }

            // Beta is the method that will be called when the work item is
            // serviced on the thread pool.
            // That means this method will be called when the thread pool has
            // an available thread for the work item.
            public void Beta(Object state)
            {
                // Write out the hashcode and cookie for the current thread
                Console.WriteLine(" {0} {1} :", Thread.CurrentThread.GetHashCode(),
                   ((SomeState)state).Cookie);
                // The lock keyword allows thread-safe modification
                // of variables accessible across multiple threads.
                Console.WriteLine(
                   "HashCount.Count=={0}, Thread.CurrentThread.GetHashCode()=={1}",
                   HashCount.Count,
                   Thread.CurrentThread.GetHashCode());
                lock (HashCount)
                {
                    if (!HashCount.ContainsKey(Thread.CurrentThread.GetHashCode()))
                        HashCount.Add(Thread.CurrentThread.GetHashCode(), 0);
                    HashCount[Thread.CurrentThread.GetHashCode()] =
                       ((int)HashCount[Thread.CurrentThread.GetHashCode()]) + 1;
                }

                // Do some busy work.
                // Note: Depending on the speed of your machine, if you 
                // increase this number, the dispersement of the thread
                // loads should be wider.
                int iX = 2000;
                Thread.Sleep(iX);
                // The Interlocked.Increment method allows thread-safe modification
                // of variables accessible across multiple threads.
                Interlocked.Increment(ref iCount);
                if (iCount == iMaxCount)
                {
                    Console.WriteLine();
                    Console.WriteLine("Setting eventX ");
                    eventX.Set();
                }
            
        }
    }
}
