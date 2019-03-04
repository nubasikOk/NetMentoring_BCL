using SorterService.ClassLibrary;
using SorterService.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Config = SorterService.Configuration.SorterServiceConfiguration;

namespace SorterService.ConsoleApp
{
    public static class FileSystemInitializer
    {

       
        private static string GetFileNameWithoutFolders(string path) => Path.GetFileName(path);
        private static string GetPathWithoutFile(string path) => Path.GetDirectoryName(path);

        private static string GenerateNewFileName(FileRelocationInfo fileRelocationInfo)
        {
            var newFileName = fileRelocationInfo.FileName;
            if (Config.Configuration.Rules.EnableCreateDateAddition)
            {
                var dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
                var date = DateTime.Now.ToString("G", dateTimeFormat);
                var separatorList = date.Where(x => !char.IsNumber(x)).ToList();
                date = separatorList.Aggregate(date, (current, separator) => current.Replace(separator, '_'));
                newFileName = $"{date}_{newFileName}";
            }
            if (!Config.Configuration.Rules.EnableAddFileIndex)
            {
                return newFileName;
            }

            var fileCount = Directory.GetFiles(fileRelocationInfo.DestinationPath).Length;
            newFileName = $"{++fileCount}_{newFileName}";

            return newFileName;
        }

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
                    GenerateNewFileName = GenerateNewFileName
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
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(ResourceManagment.GetString("EntityCreatedMsg"),
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(ResourceManagment.GetString("EntityDeletedMsg"),
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
    }
}
