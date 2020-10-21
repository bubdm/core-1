using System;
using System.Collections.Generic;
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
        public int Count { get => Items.Count; }
        public void Dispose() { }
        public int Sum(int a, int b)
        {
            return a + b;
        }

    }
}
