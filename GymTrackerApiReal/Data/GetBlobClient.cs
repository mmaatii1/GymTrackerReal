using Azure.Storage.Blobs;
using GymTracker.Shared;
using System;

namespace GymTrackerApiReal.Data
{
    public class GetBlobClient
    {
        private readonly IKeyVaultService _keyVaultService;

        public GetBlobClient(IKeyVaultService keyVault)
        {
            _keyVaultService = keyVault;
        }
        public async Task<BlobClient> GetClientBasedOnGuid(string guid)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(await _keyVaultService.GetSecret("BlobEndpoint"));
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
