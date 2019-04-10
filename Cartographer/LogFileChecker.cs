using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cartographer
{
    internal class LogFileChecker
    {
        private readonly string _filePath;
        private readonly Cartographer _cartographer;
        private Regex _regexFilter;

        internal LogFileChecker(Cartographer cartographer, string filePath)
        {
            _filePath = filePath;
            _cartographer = cartographer;

            // Generate regex
            var fileExtension = Path.GetExtension(_filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
            _regexFilter = new Regex($@"({fileNameWithoutExtension})\d+({fileExtension})");

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
            var dirInfo = new DirectoryInfo(_filePath);
            var dirPath = dirInfo.Parent.FullName;
            var fileExtension = Path.GetExtension(_filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);

            var files = dirInfo.GetFiles().Where(f => PassesFilter(f.Name));

            // rename file
            System.IO.File.Move("oldfilename", "newfilename");
        }

        private void CreateNewLogFile()
        {

        }

        private bool PassesFilter(string fileName)
        {
            return _regexFilter.IsMatch(fileName);
        }
    }
}
