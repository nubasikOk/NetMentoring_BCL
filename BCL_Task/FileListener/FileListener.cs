using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FileListener
{
    public class FileListener
    {
        private readonly string defaultDestinationDirectoryPath;
        private readonly FileSystemWorker fileSystemProvider;
        private IDictionary<Regex, string> rules;
        private Func<FileRelocationInfo, string> generateNewFileName;

       
       public FileListener()
        {

        }
    }
}
