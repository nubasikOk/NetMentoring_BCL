using System;
using System.Collections.Generic;
using System.Text;

namespace SorterService.ClassLibrary
{
    public  class RuleNotFoundEventArgs
    {
        public string FileName { get; set; }
        public string DefaultPath { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
