using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TestLibrary
{
    [Description("Открытый принтер")]
    public class Printer
    {
        private readonly string _prefix;

        public Printer([Required] string Prefix)
        {
            _prefix = Prefix;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void Print(string Message)
        {
            Console.WriteLine("{0}{1}", _prefix, Message);
        }
    }
}
