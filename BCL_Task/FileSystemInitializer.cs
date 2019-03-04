using FileListener;
using SorterServiceConfiguration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileListener
{
    public static class FileSystemInitializer
    {

        private static SorterServiceConfiguration.SorterServiceConfiguration Configuration => ConfigurationManager.GetSection("SorterServiceConfiguration") as SorterServiceConfiguration.SorterServiceConfiguration;
        private static string GetFileNameWithoutFolders(string path) => path.Split('\\').Last();
        private static string GetPathWithoutFile(string path) => path.Substring(0, path.LastIndexOf('\\'));

        private static string GenerateNewFileName(FileRelocationInfo fileRelocationInfo)
        {
            var newFileName = fileRelocationInfo.FileName;
            if (Configuration.Rules.EnableCreateDateAddition)
            {
                var dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
                var date = DateTime.Now.ToString("G", dateTimeFormat);
                var separatorList = date.Where(x => !char.IsNumber(x)).ToList();
                date = separatorList.Aggregate(date, (current, separator) => current.Replace(separator, '_'));
                newFileName = $"{date}_{newFileName}";
            }
            if (!Configuration.Rules.EnableAddFileIndex)
            {
                return newFileName;
            }

            var fileCount = Directory.GetFiles(fileRelocationInfo.DestinationPath).Length;
            newFileName = $"{++fileCount}_{newFileName}";

            return newFileName;
        }

        public static IEnumerable<FileListener> InitializeFileSystemSorters()
        {
            var patternPathDictionary = Configuration.Rules.Cast<RuleElement>()
                .ToDictionary(rule => new Regex(rule.FileNameRegexPattern), rule => rule.DestinationPath);

            var fileSystemProvider = new FileSystemWorker();

            foreach (ListenDirectoryElement directory in Configuration.ListenDirectories)
            {
                var directoryWatcher = new DirectoryWorker(directory.Path)
                {
                    IncludeSubdirectories = true
                };

                var fileSystemSorter = new FileListener(directoryWatcher,
                    Configuration.DefaultDirectory.Path, fileSystemProvider)
                {
                    Rules = patternPathDictionary,
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
