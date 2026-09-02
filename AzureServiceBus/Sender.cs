using Azure.Messaging.ServiceBus;

namespace AzureServiceBus;

public static class Sender
{
	public static async Task SendMessage(BusRequest busRequest)
	{
		var options = new ServiceBusClientOptions
		{
			TransportType = ServiceBusTransportType.AmqpWebSockets
		};
		await using (var client = new ServiceBusClient(busRequest.NameSpace, options))
		{
			var sender = client.CreateSender(busRequest.QueueName);
			var serviceBusMessage = new ServiceBusMessage(busRequest.Message);
			await sender.SendMessageAsync(serviceBusMessage);
		}
	}
}
 