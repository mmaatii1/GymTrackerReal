using Azure.Core;
using Azure.Messaging.ServiceBus;
using GymTrackerApiReal.Dtos.CustomWorkout;
using GymTrackerApiReal.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymTracker.Shared;

namespace GymTrackerApiReal.Data
{
    public class CustomWorkoutAzureBus
    {
        private readonly IKeyVaultService _keyVaultService;

        public CustomWorkoutAzureBus(IKeyVaultService keyVault)
        {
            _keyVaultService = keyVault;
        }
        public async void SendToBus(string nameOfQueue, int numOfMess, CustomWorkoutCreateUpdateDto customWorkout)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            var client = new ServiceBusClient(await _keyVaultService.GetSecret("ServiceBusEndpoint"), clientOptions);
            var sender = client.CreateSender(nameOfQueue);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMess; i++)
            {
                var des = JsonConvert.SerializeObject(customWorkout);
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Added custom workout of Id:{customWorkout.Id} , DateOfWorkout:{customWorkout.DateOfWorkout.ToString()}, Name: {customWorkout.Name}")))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }
            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMess} messages has been published to the queue.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
