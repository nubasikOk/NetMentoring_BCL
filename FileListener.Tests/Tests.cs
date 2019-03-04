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
        private IFileSystemWorker fileSystemWorkerMock;
        private IDirectoryWorker directoryWorkerMock;
        private FileListener.FileListener fileListener;
        private List<string> existingDirectoryPaths;
        private List<string> filesToAddNames;

        private readonly string directoryToWatchPath = "I://test";
        private readonly string firstLevelFolderName = "data";
        private readonly string defaultPathToMove = "default";

        [TestInitialize]
        public void Init()
        {
            fileSystemWorkerMock = MockRepository.GenerateMock<IFileSystemWorker>();
            directoryWorkerMock = MockRepository.GenerateMock<IDirectoryWorker>();
            directoryWorkerMock.Stub(x => x.Path).Return(directoryToWatchPath);
            existingDirectoryPaths = new List<string>
            {
                $"{directoryToWatchPath}",
                $"{directoryToWatchPath}/{firstLevelFolderName}"
            };

            filesToAddNames = new List<string>
            {
                "a.txt",
                "b1.txt",
                "c2.txt"
            };

            foreach (var directoryPath in existingDirectoryPaths)
            {
                fileSystemWorkerMock.Stub(provider => provider.IsDirectoryExists(directoryPath)).Return(true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileListener_ListedDirectoryIsNull_ThrowsException()
        {
            fileListener= new FileListener.FileListener(null, existingDirectoryPaths[1], fileSystemWorkerMock);
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemSorter_DefaultDirectoryIsNull_ThrowsException()
        {
            fileListener = new FileListener.FileListener(directoryWorkerMock, null, fileSystemWorkerMock);
        }




        [TestMethod]
        public void FileListener_Correct_MoveFile()
        {
            var defaultPath = Path.Combine(directoryToWatchPath, defaultPathToMove);
            var sourcePath = Path.Combine(directoryToWatchPath, filesToAddNames[0]);

            fileListener = new FileListener.FileListener(directoryWorkerMock, defaultPath, fileSystemWorkerMock);
            fileSystemWorkerMock.Stub(y => y.IsFileExists(sourcePath)).Return(true);
            fileSystemWorkerMock.Stub(x => x.MoveFile(sourcePath, defaultPath));
            directoryWorkerMock.Raise(directoryWorker => directoryWorker.Created += (s, e) => { }, new object(),
                new FileSystemEventArgs(WatcherChangeTypes.Created, directoryToWatchPath, filesToAddNames[0]));

            fileSystemWorkerMock.Expect(x => x.MoveFile(sourcePath, defaultPath));
            fileListener.RuleNotFound += (s, e) => Assert.AreEqual(defaultPath, e.DefaultPath);
        }
    }
}
