using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока


            Test_TPL.TestTPLFunctions();


            Console.WriteLine("Приложение завершило свою работу");
            Console.ReadKey();
        }
    }
}
