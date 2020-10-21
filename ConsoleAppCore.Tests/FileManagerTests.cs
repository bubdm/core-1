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
            IDataAccess access = new StubFileData();
            var manager = new FileManager(access);
            var filename = name;

            bool actual = manager.FindFile(filename);

            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void FindFile_Other_False()
        {
            IDataAccess access = new StubFileData();
            var manager = new FileManager(access);
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
    }
}
