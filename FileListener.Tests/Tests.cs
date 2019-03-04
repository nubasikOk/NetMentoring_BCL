using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SorterServiceConfiguration;
using FileListener;

namespace FileSystemSorter.Tests
{
    [TestClass]
    public class FileSystemSorterTests
    {
        private IFileSystemWorker fileSystemInfoProviderMock;
        private IDirectoryWorker directoryWatcherMock;
        private FileListener.FileListener fileListener;
        private List<string> existingDirectoryPaths;
        private List<string> filesToAddNames;

        private readonly string directoryToWatchPath = "I://test";
        private readonly string firstLevelFolderName = "data";
        private readonly string defaultPathToMove = "default";

        [TestInitialize]
        public void Init()
        {
                     
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileListener_ListedDirectoryIsNull_ThrowsException()
        {
            fileListener= new FileListener.FileListener(null, existingDirectoryPaths[1], fileSystemInfoProviderMock);
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemSorter_DefaultDirectoryIsNull_ThrowsException()
        {
            fileListener = new FileListener.FileListener(directoryWatcherMock, null, fileSystemInfoProviderMock);
        }




        [TestMethod]
        public void FileListener_Correct_MoveFile()
        {
           
        }
    }
}
