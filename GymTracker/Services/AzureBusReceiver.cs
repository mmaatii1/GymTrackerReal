using Azure.Messaging.ServiceBus;
using Plugin.LocalNotification;

namespace GymTracker.Services
{
    class AzureBusReceiver
    {
        readonly INotificationService _notificationService;
        public AzureBusReceiver(INotificationService not)
        {
            _notificationService = not;
        }
        public async Task ReciveMessage(string queName)
        {
            await using var client = new ServiceBusClient("Endpoint=sb://gymtracker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=C29I2CTgJOCADJhl1X/16tC7tO5zJs3qN+ASbNtDWCs=");

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
