using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using SorterServiceConfiguration;

namespace FileListener
{
    internal class Program
    {
        private static SorterServiceConfiguration.SorterServiceConfiguration Configuration => ConfigurationManager.GetSection("SorterServiceConfiguration") as SorterServiceConfiguration.SorterServiceConfiguration;
        internal static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(Configuration.Culture.Name);

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
