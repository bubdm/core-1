using System;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "Text.txt";
            var manager = new FileManager();

            if (manager.FindFile(filename))
                Console.WriteLine("Файл найден");
            else
                Console.WriteLine("Файл не найден");

            Console.ReadLine();
        }
    }
}
