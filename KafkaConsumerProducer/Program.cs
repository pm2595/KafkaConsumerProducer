// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaConsumerProducer;
using System.Net;

string? kafka_url = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP");


using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = kafka_url }).Build())
{
    try
    {
        await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = "purchases", ReplicationFactor = 1, NumPartitions = 1 } });
    }
    catch (CreateTopicsException e)
    {
        Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
    }
}
Thread consume = new Thread(Consumer.Consume);
Thread produce = new Thread(Producer.Produce);
consume.Start();
produce.Start();
