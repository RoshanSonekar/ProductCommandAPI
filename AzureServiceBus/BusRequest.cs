namespace AzureServiceBus;

public class BusRequest
{
	public string NameSpace { get; set; } = default!;
	public string QueueName { get; set; } = default!; 
	public string Message { get; set; } = default!;
}
