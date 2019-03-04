using System.Resources;

namespace SorterService.ConsoleApp
{
    public static class ResourceManagment
    {
      
        public static string GetString(string s)
        {
            ResourceManager rm = new ResourceManager("SorterService.ConsoleApp.Resources.Resource",
                             typeof(ResourceManagment).Assembly);

           return rm.GetString(s);
          
        }
    }
}
