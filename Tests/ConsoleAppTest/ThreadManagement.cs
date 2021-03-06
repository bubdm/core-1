using System;
using System.Threading;

namespace ConsoleAppTest
{
    static class ThreadManagement
    {
        public static void Start()
        {
            var threads = new Thread[10];
            var auto_reset_event = new AutoResetEvent(false);
            var manual_reset_event = new ManualResetEvent(false);

            for (int i = 0; i < threads.Length; i++)
            {
                var i0 = i;
                threads[i] = new Thread(() => Printer($"Сообщение {i0}", auto_reset_event));
                threads[i].Start();
            }

            Console.WriteLine("Потоки создаты. Старт?");

            

            Console.ReadKey();
            auto_reset_event.Set();

            Console.ReadKey();
            auto_reset_event.Set();

            Console.ReadKey();
            auto_reset_event.Set();

            Console.ReadKey();
            auto_reset_event.Set();

            

            Console.WriteLine("Завершено выполнение программы.");
            Console.ReadKey();
        }
        private static void Printer(string Message, EventWaitHandle EventWait)
        {
            var th_id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Поток id:{th_id} начал наботу");

            EventWait.WaitOne();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Id: {th_id}\t{Message}");
                Thread.Sleep(10);
            }

            Console.WriteLine($"Поток id:{th_id} завершил работу");
        }

    }
}