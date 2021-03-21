using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;


namespace ConsoleAppTest
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleToRussian();
            Console.WriteLine("Привет, мир!\n");

            Console.Write("Введите код на C#:> ");
            var code = Console.ReadLine();
            try
            {
                var res = await CSharpScript.EvaluateAsync(code);
                Console.WriteLine($"Результат: {res}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Что-то не так: " + e.Message);
            }

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

            var print_typ = lib.GetType("TestLibrary.Printer");
            foreach (var met in print_typ.GetMethods(BindingFlags.Instance|BindingFlags.NonPublic))
            {
                var ret_t = met.ReturnType;
                var pars = met.GetParameters();

                Console.WriteLine($"{ret_t.Name} {met.Name} ({string.Join(", ", pars.Select(p => $"{p.ParameterType.Name} {p.Name}"))})");
            }

            object printer1 = Activator.CreateInstance(print_typ, ">>>");

            var ctorpr = print_typ.GetConstructor(new []{typeof(string)});
            var printer2 = ctorpr.Invoke(new object[] {"<<<"});

            var method = print_typ.GetMethod("Print");
            method.Invoke(printer1, new []{"Привет, мир!"});

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
