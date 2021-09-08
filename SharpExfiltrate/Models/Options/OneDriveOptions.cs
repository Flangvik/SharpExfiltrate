using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Models
{
    [Verb("OneDrive", HelpText = "Exfiltrate information using the OneDrive module")]
    public class OneDriveOptions : FileOptions
    {
        [Option('u', "username", Required = true, HelpText = "Username (email) for the OneDrive account to store exfiltrated data")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "Password for the OneDrive account to store exfiltrated data")]
        public string Password { get; set; }


    }
}
