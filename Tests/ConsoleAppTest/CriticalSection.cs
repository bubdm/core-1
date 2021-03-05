using System;
using System.Linq;
using System.Threading;

namespace ConsoleAppTest
{
    internal static class CriticalSection
    {
        public static void Start()
        {
            var messages = Enumerable.Range(1, 10).Select(m => $"Message{m}").ToArray();

            for (int i = 0; i < messages.Length; i++)
            {
                var i0 = i;
                new Thread(() => PrintMessage(messages[i0], 50, true)).Start();
            }
        }

        private static void PrintMessage(string message, int count)
        {
            var thread = Thread.CurrentThread;
            for (int i = 0; i < count; i++)
            {
                Console.Write("#thId: " + thread.ManagedThreadId);
                Console.Write("#\t");
                Console.Write("#" + message);
                Console.WriteLine("#");
            }
        }

        private static readonly object __SyncRoot = new object();
        private static void PrintMessage(string message, int count, bool isLock)
        {
            if (!isLock)
            {
                PrintMessage(message, count);
                return;
            }
            var thread = Thread.CurrentThread;
            for (int i = 0; i < count; i++)
            {
                lock (__SyncRoot)
                {
                    Console.Write("#thId: " + thread.ManagedThreadId);
                    Console.Write("#\t");
                    Console.Write("#" + message);
                    Console.WriteLine("#");
                }
            }
        }
    }
}