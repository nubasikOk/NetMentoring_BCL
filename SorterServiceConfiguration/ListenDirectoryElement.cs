using System.Configuration;

namespace SorterServiceConfiguration
{
    public class ListenDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string) base["path"];
    }
}