using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Config = SorterService.Configuration.SorterServiceConfiguration;
namespace SorterService.ConsoleApp
{
    internal class Program
    {

        internal static void Main(string[] args)
        {
            

            //Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(Config.Configuration.Culture.Name);

            var fileSystemSorters = SortersInitializer.InitializeFileSystemSorters().ToList();

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
