using ConsoleAppCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("ConsoleAppCore.Tests")]
namespace ConsoleAppCore
{
    class FileManager
    {
        IDataAccess _dataAccess;
        public FileManager() : this(new FileData()) { }
        public FileManager(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public bool FindFile(string filename)
        {
            var files = _dataAccess.GetFiles();
            foreach (var file in files)
            {
                if (file.Contains(filename))
                    return true;
            }
            return false;
        }
    }
}
