﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

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


            new Thread(() =>
                {
                while (true)
                {
                    Console.Title = $"Время: {DateTime.Now.ToLongTimeString()}";
                    Thread.Sleep(100);
                }
                })
                {IsBackground = true};
            
            

            //Action<string> printer = str =>
            //{
            //    Console.WriteLine($"Поток: {Thread.CurrentThread.ManagedThreadId} - {str}");
            //    Thread.Sleep(3000);
            //    Console.WriteLine($"Завершение в {Thread.CurrentThread.ManagedThreadId}");
            //};
            //printer("Параметер"); //синх
            //printer.BeginInvoke("Параметер асинх", result =>
            //{
            //    Console.WriteLine("Завершено!");
            //}, null);
            //Func<long, long> get_factorial = Factorial;
            //get_factorial.BeginInvoke(30, result =>
            //{
            //    var y = get_factorial.EndInvoke(result);
            //    Console.WriteLine("Результат: {0}", y);
            //}, null);
            

            Console.WriteLine("Приложение завершило свою работу");
            Console.ReadKey();
        }

        private static long Factorial(long x)
        {
            var result = 1l;
            while (x > 1)
            {
                result *= x;
                x--;
                Thread.Sleep(10);
            }
            return result;
        }
    }
}
