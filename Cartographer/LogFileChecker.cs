using System;
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
        private readonly Regex _regexFilter;
        private readonly string _fileExtension;
        private readonly string _fileNameWithoutExtension;

        internal LogFileChecker(Cartographer cartographer, string filePath)
        {
            _cartographer = cartographer;
            _filePath = filePath;

            // Generate regex
            _fileExtension = Path.GetExtension(_filePath);
            _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filePath);
            _regexFilter = new Regex($@"({_fileNameWithoutExtension})\.\d+({_fileExtension})");
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
            var dirPath = Path.GetDirectoryName(_filePath);
            var dirInfo = new DirectoryInfo(dirPath);

            var files = GetOrderedFiles(dirInfo);

            foreach (var file in files)
            {
                // Get file number
                var value = file.Substring(_fileNameWithoutExtension.Length + 1, file.Length - _fileExtension.Length - _fileNameWithoutExtension.Length - 1); // Shift 1 for additional '.'
                int.TryParse(value, out var unpaddedLogFileNumber);
                
                // increment file number and add padding for easier sorting
                var paddedLogFileNumber = (unpaddedLogFileNumber + 1).ToString().PadLeft(4, '0');
                
                // rename file
                File.Move(Path.Combine(dirPath, file), Path.Combine(dirPath, $"{_fileNameWithoutExtension}" + '.' + paddedLogFileNumber + _fileExtension));
            }

            // rename file
            var newZeroFile = Path.Combine(dirPath, _fileNameWithoutExtension + ".0000" + _fileExtension);
            File.Move(_filePath, newZeroFile);
        }

        private bool PassesFilter(string fileName)
        {
            return _regexFilter.IsMatch(fileName);
        }

        private string[] GetOrderedFiles(DirectoryInfo dirInfo)
        {
            var files = dirInfo.GetFiles()
                .Where(f => PassesFilter(f.Name))
                .Select(f => f.Name)
                .ToArray();
            
            Array.Sort(files, (x, y) => string.Compare(x, y));
            
            // Reverse so that incrementing the file number does not overwrite a file with a higher number.
            Array.Reverse(files);
            return files;
        }
    }
}
