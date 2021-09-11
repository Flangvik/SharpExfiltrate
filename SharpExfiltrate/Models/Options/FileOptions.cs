using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Models
{
    public abstract class FileOptions
    {
        [Option('f', "filepath", Required = true, HelpText = "Path to file or directory to be exfiltrated")]
        public string FilePath { get; set; }

        [Option('e', "extensions", Required = false, HelpText = "Only exfiltrate files with given extensions, extension string seperated by ; (pdf;doc;xls)")]
        public string FileExtensions { get; set; }

        [Option('s', "size", Required = false, HelpText = "Max filesize in MB, all files above this number will be ignored from exfiltration.")]
        public int FileSize { get; set; }

        [Option('m', "memoryonly", Required = false, HelpText = "Create the compressed zip file entirely in memory.(Might cause OutOfMemoryException)")]
        public bool MemoryOnly { get; set; }

    }
}
