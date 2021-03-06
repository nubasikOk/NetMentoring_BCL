﻿using System.Configuration;

namespace SorterService.Configuration
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("fileNameRegexPattern", IsRequired = true, IsKey = true)]
        public string FileNameRegexPattern => (string) base["fileNameRegexPattern"];

        [ConfigurationProperty("destinationPath", IsRequired = true)]
        public string DestinationPath => (string) base["destinationPath"];
    }
}