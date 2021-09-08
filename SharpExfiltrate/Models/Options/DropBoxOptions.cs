using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Models
{

    [Verb("DropBox", HelpText = "Exfiltrate information using the DropBox module")]

    public class DropBoxOptions : FileOptions
    { 
        [Option('s', "ApiSecret", Required = true, HelpText = "Connection string to your Azure Storage Account")]
        public string ApiSecret { get; set; }

        [Option('k', "ApiKey", Required = true, HelpText = "Path to file or directory to be exfiltrated")]
        public string ApiKey { get; set; }

    }

}
