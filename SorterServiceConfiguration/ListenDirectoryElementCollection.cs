using System.Configuration;

namespace SorterServiceConfiguration
{
    [ConfigurationCollection(typeof(ListenDirectoryElement), AddItemName = "listenDirectory")]
    public class ListenDirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ListenDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ListenDirectoryElement)element).Path;
        }
    }
}
