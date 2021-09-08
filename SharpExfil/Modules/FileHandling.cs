using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfil.Modules
{
    public class FileHandling
    {
        public FileHandling(string filePath, int fileSize, string fileExtensions, bool memOnly)
        {
            FilePath = filePath;
            FileSize = fileSize;
            FileExtensions = fileExtensions;
            MemOnly = memOnly;
        }

        public string FilePath { get; set; }
        public int FileSize { get; set; }
        public string FileExtensions { get; set; }
        public bool MemOnly { get; set; }

        private string tempFilePath { get; set; } = "";
        private (Stream zipStream, int entryCount) zipStreamObject { get; set; } = (null, 0);


        public void Cleanup(Stream ZipStream)
        {

            ZipStream.Close();

            if (!string.IsNullOrEmpty(tempFilePath))
            {
                Console.WriteLine($"[+] Removing temp file {tempFilePath} ");
                try
                {
                    File.Delete(tempFilePath);
                }
                catch (Exception removeEx)
                {

                    Console.WriteLine($"[+] Failed to remove {tempFilePath} , EX: {removeEx.Message} ");
                }

            }

        }
        public (Stream zipStream, string newFileName, int entryCount) PrepareZipFile()
        {

            var password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);

            var newFileName = $"{Environment.MachineName}_{DateTime.UtcNow.ToString("yyyyMMdd'T'HHmm'UTC'")}_{Path.GetFileNameWithoutExtension(FilePath.TrimEnd('\\'))}.zip";

            string extensions = "*;";
            if (!string.IsNullOrEmpty(FileExtensions))
                extensions = FileExtensions;

            tempFilePath = "";

            if (Directory.Exists(FilePath) && !File.Exists(FilePath))
            {

                var filenames = extensions.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany(g => Directory.EnumerateFiles(FilePath, $"*.{ (g.Contains('.') ? g.Split('.')[1] : g)}", SearchOption.AllDirectories)).ToArray();
                if (MemOnly)
                    zipStreamObject = Helpers.Compression.CompressFilesMemoryStream(filenames, password, FilePath, FileSize);
                else
                {
                    var CompressFilesFileStreamResult = Helpers.Compression.CompressFilesFileStream(filenames, password, FilePath, FileSize);
                    zipStreamObject = (CompressFilesFileStreamResult.fileStream, CompressFilesFileStreamResult.entryCount);
                    tempFilePath = CompressFilesFileStreamResult.TempFilePath;
                }
            }
            else
            {

                if (MemOnly)
                    zipStreamObject = Helpers.Compression.CompressFilesMemoryStream(new string[] { FilePath }, password, "", FileSize);
                else
                {
                    var CompressFilesFileStreamResult = Helpers.Compression.CompressFilesFileStream(new string[] { FilePath }, password, "", FileSize);
                    zipStreamObject = (CompressFilesFileStreamResult.fileStream, CompressFilesFileStreamResult.entryCount);
                    tempFilePath = CompressFilesFileStreamResult.TempFilePath;

                }

            }

            if (zipStreamObject.entryCount > 0)
                Console.WriteLine($"[+] Password for Zip file is {password} ");

            return (zipStreamObject.zipStream, newFileName, zipStreamObject.entryCount);


        }
    }
}
