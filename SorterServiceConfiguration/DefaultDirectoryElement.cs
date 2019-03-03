using System.Configuration;

namespace SorterServiceConfiguration
{
    public class DefaultDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path => (string)base["path"];
    }
}