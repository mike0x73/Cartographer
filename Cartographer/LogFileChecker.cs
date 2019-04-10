using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cartographer
{
    internal class LogFileChecker
    {
        private readonly string _filePath;
        private readonly Cartographer _cartographer;
        private Regex _regexFilter;
        private readonly string _fileExtension;
        private readonly string _fileNameWithoutExtension;


        internal LogFileChecker(Cartographer cartographer, string filePath)
        {
            _cartographer = cartographer;
            _filePath = filePath;

            // Generate regex
            _fileExtension = Path.GetExtension(_filePath);
            _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
            _regexFilter = new Regex($@"({_fileNameWithoutExtension})\d+({_fileExtension})");
        }

        internal bool CheckFileRollover()
        {
            long fileSize = new FileInfo(_filePath).Length;
            if (fileSize > _cartographer.MaxFileSize)
            {
                return true;
            }

            return false;
        }

        internal void ManageLogFile()
        {
            // Get all files in directory with logFile name in
            var dirInfo = new DirectoryInfo(Path.GetDirectoryName(_filePath));
            var dirPath = dirInfo.FullName;

            var files = dirInfo.GetFiles().Where(f => PassesFilter(f.Name));

            // var newFiles = dirInfo.GetFiles();
            // var files = new List<FileInfo>();
            // 
            // foreach(var item in newFiles)
            // {
            //     if (PassesFilter(item.Name))
            //     {
            //         files.Add(item);
            //     }
            // }            

            foreach(var file in files)
            {
                var name = file.Name;
                var value = name.Substring(_fileNameWithoutExtension.Length, name.Length - _fileExtension.Length - _fileNameWithoutExtension.Length);
                int.TryParse(value, out var intNumber);
                File.Move(file.FullName, dirPath + $"{_fileNameWithoutExtension}" + intNumber + 1 + _fileExtension);
            }

            // rename file
            var newZeroFile = dirPath + "\\" + _fileNameWithoutExtension + "0" + _fileExtension;
            File.Move(_filePath, newZeroFile);
        }

        private bool PassesFilter(string fileName)
        {
            return _regexFilter.IsMatch(fileName);
        }
    }
}
