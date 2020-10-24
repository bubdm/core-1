using ConsoleAppCore.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConsoleAppCore.Tests
{
    [TestClass]
    public class FileManagerTests
    {
        [DataTestMethod]
        [DataRow("file1.txt")]
        [DataRow("file2.txt")]
        [DataRow("file3.txt")]
        public void FindFile_Files_True(string name)
        {
            IDataAccess stub = new StubFileData();
            var manager = new FileManager(stub);
            var filename = name;

            bool actual = manager.FindFile(filename);

            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void FindFile_Other_False()
        {
            IDataAccess stub = new StubFileData();
            var manager = new FileManager(stub);
            var filename = "other.oth";

            bool actual = manager.FindFile(filename);

            Assert.IsFalse(actual);
        }
        public class StubFileData : IDataAccess
        {
            public List<string> GetFiles()
            {
                var list = new List<string>
                {
                    "file1.txt", "file2.txt", "file3.txt"
                };
                return list;
            }
        }
        ////////////////////////////////////////////////////////////////////
        private FileManager _manager2;
        private MockFileData _mockFileData;
        [TestInitialize]
        public void Init()
        {
            _mockFileData = new MockFileData();
            _manager2 = new FileManager(_mockFileData);
        }
        [TestMethod]
        public void FindFile_GetFilesCalled()
        {
            _manager2.FindFile("file1.txt");

            Assert.IsTrue(_mockFileData.GetFilesCalled);
        }
        public class MockFileData : IDataAccess
        {
            public bool GetFilesCalled { get; private set; }
            public List<string> GetFiles()
            {
                GetFilesCalled = true;
                return new List<string>{"file1.txt", "file2.txt", "file3.txt"};
            }
        }


    }
}
