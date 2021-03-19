using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;


namespace ConsoleAppTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            ConsoleToRussian();
            Console.WriteLine("Привет, мир!\n");

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
            method.Invoke(printer1, new object?[] {"Привет, мир!"});
            method.Invoke(printer2, new object?[] {"Еще раз привет"});

            var field_info = print_typ.GetField("_prefix", BindingFlags.Instance | BindingFlags.NonPublic);
            var value_field = (string)field_info.GetValue(printer1);
            field_info.SetValue(printer1, "!!!WOW!!!");

            var print_deleg = (Action<string>)method.CreateDelegate(typeof(Action<string>), printer1);
            print_deleg("Тест1");
            var print_deleg2 = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), printer1, method);
            print_deleg2("Тест2");

            /// dynamic

            dynamic dPrinter1 = printer1;
            dPrinter1.Print("Тест динамического");

            var values = new object[]
            {
                "Str",
                123,
                2.12,
                true,
            };
            ProcessValue(values);

            Console.WriteLine("\nНажмите любую кнопку ...");
            Console.ReadKey();
        }

        private static void ProcessValue(IEnumerable<object> values)
        {
            foreach (dynamic value in values)
            {
                ProcessValue(value);
            }
        }

        private static void ProcessValue(string value) => Console.WriteLine($"string: {value}");
        private static void ProcessValue(int value) => Console.WriteLine($"int: {value}");
        private static void ProcessValue(double value) => Console.WriteLine($"double: {value}");
        private static void ProcessValue(bool value) => Console.WriteLine($"bool: {value}");

        private static void ConsoleToRussian()
        {
            [DllImport("kernel32.dll")] static extern bool SetConsoleCP(uint pagenum);
            [DllImport("kernel32.dll")] static extern bool SetConsoleOutputCP(uint pagenum);
            SetConsoleCP(65001);        //установка кодовой страницы utf-8 (Unicode) для вводного потока
            SetConsoleOutputCP(65001);  //установка кодовой страницы utf-8 (Unicode) для выводного потока
        }

    }
}
