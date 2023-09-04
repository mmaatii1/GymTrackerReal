using Azure.Messaging.ServiceBus;
using GymTracker.Shared;
using Plugin.LocalNotification;

namespace GymTracker.Services
{
    class AzureBusReceiver
    {
        readonly INotificationService _notificationService;
        readonly IKeyVaultService _keyVaultService;
        public AzureBusReceiver(INotificationService not,IKeyVaultService keyVault)
        {
            _keyVaultService= keyVault;
            _notificationService = not;
        }
        public async Task ReciveMessage(string queName)
        {
            await using var client = new ServiceBusClient(await _keyVaultService.GetSecret("ServiceBusEndpoint"));

            var receiver = client.CreateReceiver(queName);

            var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
            if (message != null)
            {
                var notification = new NotificationRequest
                {
                    Title = "Your workout had been added",
                    Description = message.Body.ToString(),
                    ReturningData = "Contrgratulations:D",
                };
                await _notificationService.Show(notification);
                await receiver.CompleteMessageAsync(message);
            }
            await client.DisposeAsync();
        }

    }
}
