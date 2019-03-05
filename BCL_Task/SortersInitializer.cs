using SorterService.ClassLibrary;
using SorterService.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Config = SorterService.Configuration.SorterServiceConfiguration;

namespace SorterService.ConsoleApp
{
    public static class SortersInitializer
    {


        public static IEnumerable<SorterService.ClassLibrary.FileListener> InitializeFileSystemSorters()
        {
            var fileSystemProvider = new FileSystemWorker();

            foreach (ListenDirectoryElement directory in Config.Configuration.ListenDirectories)
            {
                var directoryWatcher = new DirectoryWorker(directory.Path)
                {
                    IncludeSubdirectories = true
                };

                var fileSystemSorter = new SorterService.ClassLibrary.FileListener(directoryWatcher,
                    Config.Configuration.DefaultDirectory.Path, fileSystemProvider)
                {
                    Rules = Config.Configuration.patternPathDictionary,
                    GenerateNewFileName = NameWorker.GenerateNewFileName
                };


                fileSystemSorter.directoryWorker.Created += OnCreated;
                fileSystemSorter.directoryWorker.Changed += OnChanged;
                fileSystemSorter.directoryWorker.Deleted += OnDeleted;
                
                fileSystemSorter.RuleFound += (s, e) =>
                    Console.WriteLine(ResourceManagment.GetString("FileFoundMessage"), e.Rule, e.FileName, e.PathToMove);
                fileSystemSorter.RuleNotFound += (s, e) =>
                    Console.WriteLine(ResourceManagment.GetString("FileNotFoundMessage"), e.FileName, e.DefaultPath);

                yield return fileSystemSorter;
            }


        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(ResourceManagment.GetString("EntityChangedMsg"),
                NameWorker.GetFileNameWithoutFolders(e.Name), NameWorker.GetPathWithoutFile(e.FullPath));
        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(ResourceManagment.GetString("EntityCreatedMsg"),
                NameWorker.GetFileNameWithoutFolders(e.Name), NameWorker.GetPathWithoutFile(e.FullPath));
        }
        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(ResourceManagment.GetString("EntityDeletedMsg"),
                NameWorker.GetFileNameWithoutFolders(e.Name), NameWorker.GetPathWithoutFile(e.FullPath));
        }
    }
}
