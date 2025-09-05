using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch.Infrastructure.Options
{
    public class URLOptions
    {
        public const string OptionKey = "URL";

        public string API { get; set; }
        public string Web { get; set; }
    }
}
