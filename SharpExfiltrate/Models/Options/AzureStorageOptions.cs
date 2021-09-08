using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Models
{
    [Verb("AzureStorage", HelpText = "Exfiltrate information using the Azure Storage Account module")]
    public class AzureStorageOptions : FileOptions
    {
        [Option('c', "connectionstring", Required = true, HelpText = "Connection string to your Azure Storage Account")]
        public string ConnectionString { get; set; }

    }
}
