using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FileListener;
using SorterServiceConfiguration;

namespace FileListener
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(Configuration.Culture.Name);

            var fileSystemSorters = InitializeFileSystemSorters().ToList();

            Console.WriteLine(Resource.EnableDisableTrackingTip);
            Console.CancelKeyPress += (o, e) => Environment.Exit(0);

            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D:
                        fileSystemSorters.ForEach(sorter => sorter.directoryWorker.EnableRaisingEvents = false);
                        Console.WriteLine();
                        Console.WriteLine(Resource.TrackingStopped);
                        break;
                    case ConsoleKey.E:
                        fileSystemSorters.ForEach(sorter => sorter.directoryWorker.EnableRaisingEvents = true);
                        Console.WriteLine();
                        Console.WriteLine(Resource.TrackingStarted);
                        break;
                }
            }
        }
        private static SorterServiceConfiguration.SorterServiceConfiguration Configuration =>
            ConfigurationManager.GetSection("SorterServiceConfiguration") as SorterServiceConfiguration.SorterServiceConfiguration;
        private static string GetFileNameWithoutFolders(string path) => path.Split('\\').Last();
        private static string GetPathWithoutFile(string path) => path.Substring(0, path.LastIndexOf('\\'));
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityChangedMsg, 
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityCreatedMsg, 
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(Resource.EntityDeletedMsg,
                GetFileNameWithoutFolders(e.Name), GetPathWithoutFile(e.FullPath));
        }
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

        private static IEnumerable<FileListener> InitializeFileSystemSorters()
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
                    Console.WriteLine(Resource.FileFoundMessage, e.Rule, e.FileName, e.PathToMove);
                fileSystemSorter.RuleNotFound += (s, e) => 
                    Console.WriteLine(Resource.FileNotFoundMessage, e.FileName, e.DefaultPath);

                yield return fileSystemSorter;
            }
        }
    }
}