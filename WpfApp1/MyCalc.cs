using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfApp1
{
    [Serializable]
    public class MyCalc : IDisposable
    {
        public List<int> Items = new List<int>();
        public void Add(int item)
        {
            Items.Add(item);
        }
        public void Remove(int index)
        {
            Items.RemoveAt(index);
        }
        public int Count => Items.Count;
        public void Dispose() { }
        public int Sum(int a, int b)
        {
            return a + b;
        }
        public static double GetSqrt(double x)
        {
            return Math.Sqrt(x);
        }
        public string SayHello(string name)
        {
            if (name == null)
                throw new ArgumentNullException("Null parameter " + nameof(name));
            return $"Привет {name}!";
        }
        public string MakeTemplate(string name, string level)
        {
            string path = Path.Combine(".","Template.tmpl");
            string template = File.ReadAllText(path);
            template = template.Replace("{Name}", name);
            template = template.Replace("{Level}", level);
            return template;
        }
    }
}
