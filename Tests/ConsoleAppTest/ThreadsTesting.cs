using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Threads
    {

        public static void Start()
        {
            var thread = new Thread(ThreadMethod);
            thread.Priority = ThreadPriority.Highest;
            thread.Name = "Clock Thread";
            thread.Start();

            var message = "Мое сообщение";

            var thread2 = new Thread(() => Printer("новое")) {Priority = ThreadPriority.Lowest};
            thread2.Start();

            new Thread(PrintObjInfo){Priority = ThreadPriority.Highest}.Start(message);

            var printerTask = new PrintTask(500, "123");
            printerTask.Start();

            thread2.Join();
            //if (!thread2.Join(1000))
            //    thread2.Interrupt();
        }

        private static void ThreadMethod()
        {
            while (true)
            {
                Console.Title = DateTime.Now.ToString("HH:mm:ss:ffff");
                Thread.Sleep(10);
            }
        }
        class PrintTask
        {
            private readonly int _id;
            private readonly string _message;
            private readonly Thread _Thread;

            public PrintTask(int id, string Message)
            {
                _id = id;
                _message = Message;
                _Thread = new Thread(ThreadMethod);
            }
            private void ThreadMethod()
            {
                for (int i = 0; i < 200; i++)
                {
                    Console.WriteLine($"Mid:{_id} -- {_message}");
                    Thread.Sleep(10);
                }
            }
            public void Start()
            {
                _Thread.Start();
            }
        }

        private static void PrintThreadInfo()
        {
            var thread = Thread.CurrentThread;
            Console.WriteLine($"id {thread.ManagedThreadId} name {thread.Name} priority {thread.Priority} ");
        }

        private static void Printer(string Message)
        {
            PrintThreadInfo();

            var threadid = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"id: {threadid} Mes: {Message}");
                Thread.Sleep(10);
            }
        }

        private static void PrintObjInfo(object obj)
        {
            PrintThreadInfo();

            var Message = (string) obj;

            var threadid = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"id: {threadid} Mes: {Message}");
                Thread.Sleep(10);
            }
        }
    }
}
