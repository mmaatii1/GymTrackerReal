using Azure.Storage.Blobs;
using System;

namespace GymTrackerApiReal.Data
{
    public static class GetBlobClient
    {
        public static BlobClient GetClientBasedOnGuid(string guid)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=gymtrackerstorage;AccountKey=Ln2Ijx26rp8tFA3prjufPWBj5M1KyVkrHdVXCFx0J/+Bqo8FeXgeH157vKSfJOhiwDbce+2KRnob+AStWOjvEA==;EndpointSuffix=core.windows.net");
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("trainingphotos");
                BlobClient blobClient = containerClient.GetBlobClient(guid);
                return blobClient;
            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
