using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleAppCore.DataAccess
{
    class FileData : IDataAccess
    {
        public string Path { get; set; }
        public List<string> GetFiles()
        {
            string path = Directory.GetCurrentDirectory();
            return getFilesForPath(path);
        }
        public List<string> GetFiles(string path) => getFilesForPath(path);

        private static List<string> getFilesForPath(string path)
        {
            var list = new List<string>();
            var files = new DirectoryInfo(path).GetFiles();
            foreach (var file in files)
            {
                list.Add(file.Name);
            }
            return list;
        }
    }
    public interface IDataAccess
    {
        public string Path { get; set; }
        List<string> GetFiles();
        List<string> GetFiles(string path);
    }
}
