using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfil.Models
{

    [Verb("AzureStorage", HelpText = "Exfiltrate information using the OneDrive module")]
    public class DropBoxOptions
    { 
        [Option('c', "connectionstring", Required = true, HelpText = "Connection string to your Azure Storage Account")]
        public string Username { get; set; }

        [Option('f', "filepath", Required = true, HelpText = "Path to file or directory to be exfiltrated")]
        public string FilePath { get; set; }

        [Option('e', "extensions", Required = false, HelpText = "string of file extensions seperated by ; to filter on (pdf;doc;xls)")]
        public string FileExtensions { get; set; }

        [Option('s', "size", Required = false, HelpText = "Set a max filesize in MB, all files above this number will be ignored.")]
        public int FileSize { get; set; }

        [Option('m', "memoryonly", Required = false, HelpText = "Will create the compressed zip file entirely out from memory.(Might cause OutOfMemoryException)")]
        public bool MemOnly { get; set; }
    }

}
