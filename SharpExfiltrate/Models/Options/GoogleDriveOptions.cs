using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Models
{

    [Verb("GoogleDrive", HelpText = "Exfiltrate information using the GoogleDrive module")]
    public class GoogleDriveOptions : FileOptions
    { 

        [Option('a', "appname", Required = true, HelpText = "Connection string to your Azure Storage Account")]
        public string Appname { get; set; }

        [Option('c', "accesstoken", Required = true, HelpText = "Connection string to your Azure Storage Account")]
        public string AccessToken { get; set; }
    }


}
