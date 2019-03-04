using System.Resources;

namespace FileListener
{
    public static class ResourceManagment
    {
      
        public static string GetString(string s)
        {
            ResourceManager rm = new ResourceManager("BCL_Task.Resources.Resource",
                             typeof(ResourceManagment).Assembly);

           return rm.GetString(s);
          
        }
    }
}
