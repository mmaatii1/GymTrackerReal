using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace GymTracker.Shared
{
    public class KeyVaultService : IKeyVaultService
    {
        public async Task<string> GetSecret(string secretName)
        {
            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret =  await client.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}