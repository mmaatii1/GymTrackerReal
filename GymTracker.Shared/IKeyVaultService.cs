
namespace GymTracker.Shared
{
    public interface IKeyVaultService
    {
        Task<string> GetSecret(string secretName);
    }
}
