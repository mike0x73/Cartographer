using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cartographer
{
    internal class LogFileChecker
    {
        private readonly string _filePath;
        private readonly Cartographer _cartographer;

        internal LogFileChecker(Cartographer cartographer, string filePath)
        {
            _filePath = filePath;
            _cartographer = cartographer;
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
            // rename file
            System.IO.File.Move("oldfilename", "newfilename");
        }

        private void CreateNewLogFile()
        {

        }
    }
}
