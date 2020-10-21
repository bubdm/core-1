using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace WpfApp1.Tests
{
    [TestClass]
    public class MyCalcTests
    {
        private MyCalc myCalc;
        private int item;
        [TestInitialize]
        public void TestInitialize()
        {
            Debug.WriteLine("Test Init");
            item = 10;
            myCalc = new MyCalc();
            myCalc.Add(item);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Debug.WriteLine("TestStop");
            myCalc.Dispose();
        }
        [TestMethod]
        public void Check_Constains_Item_In_Items()
        {
            CollectionAssert.Contains(myCalc.Items, item);
        }
        [TestMethod]
        public void Check_Remove_Item_From_Items()
        {
            myCalc.Remove(0);
            CollectionAssert.DoesNotContain(myCalc.Items, item);
        }
        [TestMethod]
        public void Sum_10_20_equal_30()
        {
            int x = 10;
            int y = 20;
            int expected = 30;

            
            int actual = myCalc.Sum(x, y);

            Assert.AreEqual(expected, actual);
        }
    }
}
