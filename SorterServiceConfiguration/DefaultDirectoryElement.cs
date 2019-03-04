using System.Configuration;

namespace SorterService.Configuration
{
    public class DefaultDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path => (string)base["path"];
    }
}