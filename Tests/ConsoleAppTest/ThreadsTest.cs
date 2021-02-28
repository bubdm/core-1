using System;
using System.Linq;
using System.Threading;

namespace ConsoleAppTest
{
    class ThreadsTest
    {
        public static void Start()
        {
            var messages = Enumerable.Range(1, 100).Select(m => $"Message{m}").ToArray();


            
            for (int i = 0; i < messages.Length - 1; i++)
            {
                var i0 = i;
                ThreadPool.QueueUserWorkItem(_ => { PrintMessage(messages[i0]); });
            }
        }

        private static void PrintMessage(string message)
        {
            Console.WriteLine($"id:{Thread.CurrentThread.ManagedThreadId} Mes:{message}");
            Thread.Sleep(3);
        }
    }
}