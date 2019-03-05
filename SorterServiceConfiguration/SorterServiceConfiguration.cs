using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace SorterService.Configuration
{
    public class SorterServiceConfiguration : ConfigurationSection
    {
        public static SorterServiceConfiguration Configuration
        {
            get => ConfigurationManager.GetSection("SorterService.Configuration") as SorterServiceConfiguration;
        }

        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules => (RuleElementCollection)base["rules"];

        public  Dictionary<Regex, string> patternPathDictionary {
           get=> Configuration.Rules.Cast<RuleElement>()
                 .ToDictionary(rule => new Regex(rule.FileNameRegexPattern), rule => rule.DestinationPath);
            }

        [ConfigurationProperty("culture")]
        public CultureElement Culture => (CultureElement)this["culture"];
        [ConfigurationProperty("defaultDirectory")]
        public DefaultDirectoryElement DefaultDirectory => (DefaultDirectoryElement)this["defaultDirectory"];

        [ConfigurationProperty("listenDirectories")]
        public ListenDirectoryElementCollection ListenDirectories => (ListenDirectoryElementCollection)base["listenDirectories"];
    }
}
