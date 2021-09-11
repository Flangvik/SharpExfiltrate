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

        [Option('n', "appname", Required = true, HelpText = "GoogleDrive Application name (Can be anything)")]
        public string AppName { get; set; }

        [Option('t', "accesstoken", Required = true, HelpText = "Valid access token onbehalf of your GoogleDrive account")]
        public string AccessToken { get; set; }
    }


}
