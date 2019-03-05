using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SorterService.ConsoleApp;
using SorterService.ClassLibrary;
using System.Globalization;


namespace SorterService.Tests
{
    [TestClass]
    public class FileSystemSorterTests
    {
        private IFileSystemWorker fileSystemWorkerMock;
        private IDirectoryWorker directoryWorkerMock;
        private SorterService.ClassLibrary.FileListener fileListener;
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
            fileListener= new SorterService.ClassLibrary.FileListener(null, existingDirectoryPaths[1], fileSystemWorkerMock);
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemSorter_DefaultDirectoryIsNull_ThrowsException()
        {
            fileListener = new SorterService.ClassLibrary.FileListener(directoryWorkerMock, null, null);
        }




        [TestMethod]
        public void FileListener_Correct_MoveFile_With_NoRule()
        {
            var defaultPath = Path.Combine(directoryToWatchPath, defaultPathToMove);
            var sourcePath = Path.Combine(directoryToWatchPath, filesToAddNames[0]);

            fileListener = new SorterService.ClassLibrary.FileListener(directoryWorkerMock, defaultPath, fileSystemWorkerMock);
            fileSystemWorkerMock.Stub(y => y.IsFileExists(sourcePath)).Return(true);
            fileSystemWorkerMock.Stub(x => x.MoveFile(sourcePath, defaultPath));
            directoryWorkerMock.Raise(directoryWorker => directoryWorker.Created += (s, e) => { }, new object(),
                new FileSystemEventArgs(WatcherChangeTypes.Created, directoryToWatchPath, filesToAddNames[2]));

            fileSystemWorkerMock.Expect(x => x.MoveFile(sourcePath, defaultPath));
            fileListener.RuleNotFound += (s, e) => Assert.AreEqual(defaultPath, e.DefaultPath);
        }



        [TestMethod]
        public void FileListener_Correct_MoveFile_With_Rules()
        {
            var defaultPath = Path.Combine(directoryToWatchPath, defaultPathToMove);
            var sourcePath = Path.Combine(directoryToWatchPath, filesToAddNames[0]);
            var targetPath = Path.Combine(existingDirectoryPaths[1], filesToAddNames[0]);
            var rules = new Dictionary<Regex, string> { { new Regex("[a-c]"), existingDirectoryPaths[1] } };
            fileListener = new SorterService.ClassLibrary.FileListener(directoryWorkerMock, defaultPath, fileSystemWorkerMock)
            {
                Rules = rules
            };

            fileSystemWorkerMock.Stub(y => y.IsFileExists(sourcePath)).Return(true);
            directoryWorkerMock.Raise(directoryWatcher => directoryWatcher.Created += (s, e) => { }, new object(),
                new FileSystemEventArgs(WatcherChangeTypes.Created, directoryToWatchPath, filesToAddNames[0]));


            fileSystemWorkerMock.Expect(x => x.MoveFile(sourcePath, targetPath));
            
            fileListener.RuleFound += (s, e) => Assert.AreEqual(targetPath, e.PathToMove);
        }

        [TestMethod]
        public void Application_LocalizationRu_correct()
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("ru-RU");
            Assert.AreEqual(ResourceManagment.GetString("EnableDisableTrackingTip"), "Нажмите клавишу для зап[У]ска/остано[В]ки отслеживания каталога:");
        }

        [TestMethod]
        public void Application_LocalizationEN_correct()
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            Assert.AreEqual(ResourceManagment.GetString("EnableDisableTrackingTip"), "Press key to [E]nable/[D]isable tracking");
        }

        [TestMethod]
        public void Application_Localization_NoResource_correct()
        {
            Assert.AreEqual(ResourceManagment.GetString("123"), null);
        }



        [TestMethod]
        public void Application_Localization_If_UnknownCulture_Then_DefaultCulture_Using_correct()
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("ja-JP");
            Assert.AreEqual(ResourceManagment.GetString("EnableDisableTrackingTip"), "Press key to [E]nable/[D]isable tracking");
        }
    }
}
