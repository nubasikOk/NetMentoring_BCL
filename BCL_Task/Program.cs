using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using SorterService.Configuration;
using SorterService.ConsoleApp;
using Config = SorterService.Configuration.SorterServiceConfiguration;
namespace SorterService.ConsoleApp
{
    internal class Program
    {

        internal static void Main(string[] args)
        {
            

            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(Config.Configuration.Culture.Name);

            var fileSystemSorters = FileSystemInitializer.InitializeFileSystemSorters().ToList();

            Console.WriteLine(ResourceManagment.GetString("EnableDisableTrackingTip"));
            Console.CancelKeyPress += (o, e) => Environment.Exit(0);

            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D:
                        fileSystemSorters.ForEach(sorter => sorter.directoryWorker.EnableRaisingEvents = false);
                        Console.WriteLine();
                        Console.WriteLine(ResourceManagment.GetString("TrackingStopped"));
                        break;
                    case ConsoleKey.E:
                        fileSystemSorters.ForEach(sorter => sorter.directoryWorker.EnableRaisingEvents = true);
                        Console.WriteLine();
                        Console.WriteLine(ResourceManagment.GetString("TrackingStarted"));
                        break;
                }
            }
        }


       

       

    }
}
