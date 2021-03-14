using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

            const string file = "TestLibrary.dll";
            Assembly lib = Assembly.LoadFile(Path.GetFullPath(file)); //полная загрузка
            //Assembly thisAssembly = Assembly.GetEntryAssembly(); //загрузить сам себя
            var asm2 = typeof(Program).Assembly;
            var path = asm2.Location;

            var type = lib.DefinedTypes.First();

            var props = type.GetProperties();
            var mets = type.GetMethods();
            var ctors = type.GetConstructors();
            var evs = type.GetEvents();
            var fiels = type.GetFields();

            

            Console.WriteLine("Нажмите любую кнопку ...");
            Console.ReadKey();
        }
    }
}
