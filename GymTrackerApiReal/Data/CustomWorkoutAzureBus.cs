using Azure.Core;
using Azure.Messaging.ServiceBus;
using GymTrackerApiReal.Dtos.CustomWorkout;
using GymTrackerApiReal.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymTrackerApiReal.Data
{
    public static class CustomWorkoutAzureBus
    {
        public static async void SendToBus(string nameOfQueue, int numOfMess, CustomWorkoutCreateUpdateDto customWorkout)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            var client = new ServiceBusClient("Endpoint=sb://gymtracker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=C29I2CTgJOCADJhl1X/16tC7tO5zJs3qN+ASbNtDWCs=", clientOptions);
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
