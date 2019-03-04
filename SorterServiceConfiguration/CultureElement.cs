using System.Configuration;

namespace SorterService.Configuration
{
    public class CultureElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name => (string)base["name"];
    }
}
