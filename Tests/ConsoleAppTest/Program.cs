using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;



namespace ConsoleAppTest
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleToRussian();
            Console.WriteLine("Привет, мир!\n");

 
            Console.WriteLine("\nНажмите любую кнопку ...");
            Console.ReadKey();
        }


        private static void ConsoleToRussian()
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока
        }

    }
}
