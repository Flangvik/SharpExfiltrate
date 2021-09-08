using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Helpers
{
    public static class Compression
    {

        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }


        public static (FileStream fileStream, string TempFilePath, int entryCount) CompressFilesFileStream(string[] filePaths, string password, string rootDirectory = "", int maxFileSize = 0)
        {
            int entryCount = 0;
            //If we try to store all this in memory we WILL get an out of memory expection, so sadly we have to drop to disk...
            var tempFile = Path.GetTempFileName();
            var zipFileStream = new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            using (ZipOutputStream zipStream = new ZipOutputStream(zipFileStream))
            {

                zipStream.SetLevel(9);
                zipStream.Password = password;

                foreach (string filePath in filePaths)
                {
                    try
                    {


                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            if ((fileStream.Length / 1048576.0) <= maxFileSize || maxFileSize == 0)
                            {
                                ZipEntry newEntry = null;
                                if (string.IsNullOrEmpty(rootDirectory))
                                {
                                    newEntry = new ZipEntry(Path.GetFileName(filePath));
                                    Console.WriteLine($"[+] Compressing {filePath} {BytesToString(fileStream.Length)}");
                                }
                                else
                                {
                                    Console.WriteLine($"[+] Compressing {filePath.Substring(rootDirectory.Length).TrimStart('\\')} {BytesToString(fileStream.Length)}");
                                    newEntry = new ZipEntry(filePath.Substring(rootDirectory.Length).TrimStart('\\'));
                                }
                                newEntry.DateTime = DateTime.UtcNow;

                                zipStream.PutNextEntry(newEntry);
                                fileStream.CopyTo(zipStream);
                                zipStream.CloseEntry();
                                entryCount++;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(rootDirectory))
                            Console.WriteLine($"[!] Failed to compress {filePath} , file locked by another process?");
                        else
                            Console.WriteLine($"[!] Failed to compress {filePath.Substring(rootDirectory.Length).TrimStart('\\')}, file locked by another process?");
                    }

                }

                zipStream.IsStreamOwner = false;
                zipStream.Close();

            }
            zipFileStream.Position = 0;


            return (zipFileStream, tempFile, entryCount);



        }
        //https://stackoverflow.com/questions/8624071/save-and-load-memorystream-to-from-a-file
        public static (MemoryStream memStream, int entryCount) CompressFilesMemoryStream(string[] filePaths, string password, string rootDirectory = "", int maxFileSize = 0)
        {

            int entryCount = 0;
            MemoryStream outputMemStream = new MemoryStream();

            using (ZipOutputStream zipStream = new ZipOutputStream(outputMemStream))
            {

                zipStream.SetLevel(9);
                zipStream.Password = password;

                foreach (var filePath in filePaths)
                {
                    try
                    {

                        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            if ((file.Length / 1048576.0) <= maxFileSize || maxFileSize == 0)
                            {
                                ZipEntry newEntry = null;
                                if (string.IsNullOrEmpty(rootDirectory))
                                {
                                    newEntry = new ZipEntry(Path.GetFileName(filePath));
                                    Console.WriteLine($"[+] Compressing {filePath} {BytesToString(file.Length)}");
                                }
                                else
                                {
                                    Console.WriteLine($"[+] Compressing {filePath.Substring(rootDirectory.Length).TrimStart('\\')} {BytesToString(file.Length)}");
                                    newEntry = new ZipEntry(filePath.Substring(rootDirectory.Length).TrimStart('\\'));
                                }
                                newEntry.DateTime = DateTime.UtcNow;

                                zipStream.PutNextEntry(newEntry);
                                entryCount++;
                                StreamUtils.Copy(file, zipStream, new byte[4096]);
                                zipStream.CloseEntry();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(rootDirectory))
                            Console.WriteLine($"[!] Failed to compress {filePath} , file locked by another process?");
                        else
                            Console.WriteLine($"[!] Failed to compress {filePath.Substring(rootDirectory.Length).TrimStart('\\')}, file locked by another process?");
                    }
                }

                zipStream.IsStreamOwner = false;
                zipStream.Close();

                outputMemStream.Position = 0;



                return (outputMemStream, entryCount);
            }

        }
    }
}


