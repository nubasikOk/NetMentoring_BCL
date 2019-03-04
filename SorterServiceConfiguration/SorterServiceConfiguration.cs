using System.Configuration;

namespace SorterService.Configuration
{
    public class SorterServiceConfiguration : ConfigurationSection
    {
        public static SorterService.Configuration.SorterServiceConfiguration Configuration => ConfigurationManager.GetSection("SorterService.Configuration") as SorterService.Configuration.SorterServiceConfiguration;
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules => (RuleElementCollection)base["rules"];

        [ConfigurationProperty("culture")]
        public CultureElement Culture => (CultureElement)this["culture"];
        [ConfigurationProperty("defaultDirectory")]
        public DefaultDirectoryElement DefaultDirectory => (DefaultDirectoryElement)this["defaultDirectory"];

        [ConfigurationProperty("listenDirectories")]
        public ListenDirectoryElementCollection ListenDirectories => (ListenDirectoryElementCollection)base["listenDirectories"];
    }
}
