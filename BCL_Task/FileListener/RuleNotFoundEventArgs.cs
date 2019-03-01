using System;
using System.Collections.Generic;
using System.Text;

namespace FileListener
{
    class RuleNotFoundEventArgs
    {
        public string FileName { get; set; }
        public string DefaultPath { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
