using System;
using System.ComponentModel;

namespace TestLibrary
{
    [Description("Скрытый принтер")]
    internal class InternalPrinter : Printer
    {
        public int Value { get; private set; } = 5;
        public InternalPrinter() : base("internal:")
        {
        }
        public override void Print(string Message)
        {
            Console.WriteLine("Private print = {0}", Value++);
            base.Print(Message);
        }
    }
}