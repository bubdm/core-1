using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace ConsoleAppTest
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            #region Русификация консоли обязательная
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока
            #endregion

            
            Console.WriteLine("Нажмите любую кнопку ...");
            Console.ReadKey();
        }
    }
}
