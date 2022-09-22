using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Net;

namespace KafkaConsumerProducer
{
    internal class KafkaConnection
    {

        ProducerConfig ProducerConfigKafka { get;  }
        ConsumerConfig ConsumerconfigKafka { get; }

        public KafkaConnection()
        {
            string? kafka_url = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP");
            if (kafka_url == null) throw new Exception("Please set the envionment variable KAFKA_BOOTSTRAP");
            ProducerConfigKafka = new ProducerConfig
            {
                BootstrapServers = kafka_url,
                ClientId = Dns.GetHostName()
            };
            ConsumerconfigKafka = new ConsumerConfig
            {
                BootstrapServers = kafka_url,
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

        }

    }
}
