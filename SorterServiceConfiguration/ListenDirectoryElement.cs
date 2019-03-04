using System.Configuration;

namespace SorterService.Configuration
{
    public class ListenDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string) base["path"];
    }
}