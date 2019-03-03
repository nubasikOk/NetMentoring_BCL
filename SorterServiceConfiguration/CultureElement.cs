using System.Configuration;

namespace SorterServiceConfiguration
{
    public class CultureElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name => (string)base["name"];
    }
}
