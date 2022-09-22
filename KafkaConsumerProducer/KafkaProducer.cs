using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumerProducer
{
    class Producer
    {
        internal static void Produce()
        {

            string? kafka_url = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP");

            var configuration = new ProducerConfig
            {
                BootstrapServers = kafka_url,
                ClientId = Dns.GetHostName()
            };

            const string topic = "purchases";

            string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
            string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

            using (var producer = new ProducerBuilder<string, string>(
                configuration.AsEnumerable()).Build())
            {
                var numProduced = 0;
                Random rnd = new Random();
                const int numMessages = 10;
                for (int i = 0; i < numMessages; ++i)
                {
                    var user = users[rnd.Next(users.Length)];
                    var item = items[rnd.Next(items.Length)];

                    producer.Produce(topic, new Message<string, string> { Key = user, Value = item },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
                                numProduced += 1;
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        }
    }
}
