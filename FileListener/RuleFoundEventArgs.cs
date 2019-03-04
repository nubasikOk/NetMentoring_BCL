using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SorterService.ClassLibrary
{
    public class RuleFoundEventArgs
    {
        public Regex Rule { get; set; }
        public string PathToMove { get; set; }
        public string FileName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
