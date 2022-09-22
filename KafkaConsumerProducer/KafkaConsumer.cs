using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

class Consumer
{

    internal static void Consume()
    {
        string? kafka_url = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP");

        var configuration = new ConsumerConfig
        {
            BootstrapServers = kafka_url,
            GroupId = "pruchases",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };


        const string topic = "purchases";

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true; // prevent the process from terminating.
            cts.Cancel();
        };

        using (var consumer = new ConsumerBuilder<string, string>(
            configuration.AsEnumerable()).Build())
        {
            bool topicExists = false;
            while (!topicExists)
            {
                try
                {
                    consumer.Subscribe(topic);
                    topicExists = true;
                }
                catch (Confluent.Kafka.ConsumeException e)
                {

                    Thread.Sleep(5);
                } 
            }
            try
            {
                while (true)
                {
                    var cr = consumer.Consume(cts.Token);
                    Console.WriteLine($"Consumed event from topic {topic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                // Ctrl-C was pressed.
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
