namespace Invoice.API.Configs
{
    public class ServiceBusConfig
    {
        public string ServiceBusConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
