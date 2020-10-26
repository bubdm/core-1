using ConsoleAppCore.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Linq;

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
            public string Path { get; set; }
            public List<string> GetFiles()
            {
                var list = new List<string>
                {
                    "file1.txt", "file2.txt", "file3.txt"
                };
                return list;
            }
            public List<string> GetFiles(string path) => GetFiles();
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
            public string Path { get; set; }
            public List<string> GetFiles()
            {
                GetFilesCalled = true;
                return new List<string>{"file1.txt", "file2.txt", "file3.txt"};
            }
            public List<string> GetFiles(string path) => GetFiles();
        }
        [TestMethod]
        public void FindFile_GetFilesOne()
        {
            var stab = Mock.Of<IDataAccess>(d => d.GetFiles() == new List<string> { "file1.txt" });
            var manager = new FileManager(stab);

            var actual = manager.FindFile("file1.txt");

            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void FindFile_GetFilesAny()
        {
            var stab = Mock.Of<IDataAccess>(d => d.GetFiles(It.IsAny<string>()) == new List<string> { "file1.txt" });

            var actual = stab.GetFiles("123");

            CollectionAssert.AreEqual(new List<string>{ "file1.txt" }, actual);
        }
        [TestMethod]
        public void FindFile_GetFilesCurr()
        {
            var stab = new Mock<IDataAccess>();
            stab.Setup(d => d.GetFiles(It.IsAny<string>())).Returns<string>(name => new List<string> { "123" + name });

            const string str = "Name";
            var actual = stab.Object.GetFiles(str);

            CollectionAssert.AreEqual(new List<string> { "123" + str }, actual);
        }
        [TestMethod]
        public void FindFile_StabProp()
        {
            var stab = Mock.Of<IDataAccess>(d => d.Path == "1");

            var actual = stab.Path;

            Assert.AreEqual("1", actual);
        }
        [TestMethod]
        public void FindFile_MultiStab()
        {
            var stab = Mock.Of<IDataAccess>(d => d.Path == "1" 
            && d.GetFiles() == new List<string> { "file1.txt" });

            Assert.AreEqual("1", stab.Path);
            CollectionAssert.AreEqual(new List<string> { "file1.txt" }, stab.GetFiles());
        }
        [TestMethod]
        public void FindFile_GetFilesIsTrue()
        {
            var mock = new Mock<IDataAccess>();
            mock.Setup(a => a.GetFiles()).Returns(new List<string> {"file1.txt"});
            var manager = new FileManager(mock.Object);

            var result = manager.FindFile("file1.txt");

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void FindFile_GetFilesCalledMock()
        {
            var mock = new Mock<IDataAccess>();
            mock.Setup(d => d.GetFiles()).Returns(new List<string> { "file1.txt" });
            var manager = new FileManager(mock.Object);

            manager.FindFile("test1.txt");

            mock.Verify(da => da.GetFiles(), Times.Once);
        }
        [TestMethod]
        public void FindFile_GetFileDefault()
        {
            var mock = new Mock<IDataAccess>();
            mock.Setup(d => d.GetFiles()).Returns(new List<string> { "file1.txt" });
            var manager = new FileManager(mock.Object);

            manager.FindFile("file1.txt");

            mock.Verify();
        }
        [TestMethod]
        public void FindFile_GetFileDefaultDouble()
        {
            var mock = new Mock<IDataAccess>(MockBehavior.Strict);
            mock.Setup(d => d.GetFiles()).Returns(new List<string> { "file1.txt" });
            var manager = new FileManager(mock.Object);

            manager.FindFile("file1.txt");

            mock.Verify();
        }
        [TestMethod]
        public void FindFile_GetFileMockRep()
        {
            var repository = new MockRepository(MockBehavior.Default);
            var mock = repository.Of<IDataAccess>()
                .Where(ld => ld.Path == "1")
                .Where(ld => ld.GetFiles() == new List<string> { "file1.txt" }).First();

            Assert.AreEqual("1", mock.Path);
            CollectionAssert.AreEqual(new List<string> { "file1.txt" }, mock.GetFiles());
        }
        ///////////////////////////////////////////////////////////////////////



    }
}
