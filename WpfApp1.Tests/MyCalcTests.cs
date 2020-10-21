using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WpfApp1.Tests
{
    [TestClass]
    public class MyCalcTests
    {
        public TestContext Context { get; set; }
        private MyCalc myCalc;
        private int item;
        static ICollection<string> employees;
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
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            employees = new List<string> { "Иван", "Вася", "Петя" };
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
        [DataTestMethod]
        [DataRow(1.0, 1.0)]
        [DataRow(4.0, 2.0)]
        [DataRow(9.0, 3.0)]
        [DataRow(16.0, 4.0)]
        public void GetSqrt_Arr_return_Arr(double value, double expected)
        {
            var actual = MyCalc.GetSqrt(value);

            Assert.AreEqual(expected, actual, $"GetSqrt({value}) должен был вернуть {expected}, а не {actual}");
        }
        [TestMethod]
        public void GetSqrt_10_return__3_1__d__0_1()
        {
            var value = 10;
            var expected = 3.1;
            var delta = 0.1;

            var actual = MyCalc.GetSqrt(value);

            Assert.AreEqual(expected, actual, delta, "Облом!");
        }
        [TestMethod]
        public void StringAreEqual()
        {
            const string inp = "hello";
            const string expected = "HELLO";
            Assert.AreEqual(expected, inp, true, "Облом со сравнением строк!");
        }
        [TestMethod]
        public void StringAreSame()
        {
            string a = "hello";
            string b = "hello";
            Assert.AreSame(a, b);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SayHello_Exception()
        {
            myCalc.SayHello(null);
        }
        [TestMethod]
        public void Test_AreNotNull()
        {
            CollectionAssert.AllItemsAreNotNull((System.Collections.ICollection)employees);
        }
        [TestMethod]
        public void Test_AreUnique()
        {
            CollectionAssert.AllItemsAreUnique((System.Collections.ICollection)employees);
        }
        [TestMethod]
        public void Test_EmployeesAreEqual()
        {
            var expected = new List<string> {"Иван", "Вася", "Петя"};

            CollectionAssert.AreEqual(expected, (System.Collections.ICollection)employees, "Коллекции не равны");
        }
        [TestMethod]
        public void Test_EmployeesAreEquivalent()
        {
            var expected = new List<string> { "Иван", "Петя", "Вася" };

            CollectionAssert.AreEquivalent(expected, (System.Collections.ICollection)employees, "Элементы в коллекциях разные");
        }
        [TestMethod]
        public void TestSubset()
        {
            var expected = new List<string> { "Петя", "Вася" };

            CollectionAssert.IsSubsetOf(expected, (System.Collections.ICollection)employees, "Облом!");
        }
        [DataTestMethod]
        [DataRow("1234567890", @"\d3")]
        [DataRow("123", @"\d3")]
        public void TestMaches(string value, string mask)
        {
            StringAssert.Matches(value, new Regex(mask));
        }
        [TestMethod]
        public void TestSayHello_Hello()
        {
            const string expected = "Привет Андрей!";

            var actual = myCalc.SayHello("Андрей");

            StringAssert.Equals(expected, actual);
        }

    }
}
