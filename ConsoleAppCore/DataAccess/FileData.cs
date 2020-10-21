using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleAppCore.DataAccess
{
    class FileData : IDataAccess
    {
        public List<string> GetFiles()
        {
            string path = Directory.GetCurrentDirectory();
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
        List<string> GetFiles();
    }
}
