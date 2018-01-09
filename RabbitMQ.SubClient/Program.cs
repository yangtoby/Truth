using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.SubClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.AutomaticRecoveryEnabled = true;
            using (var connection = factory.CreateConnection())
            {
                var exchange = "fanoutexchange";
                var routingkey = "fanoutkey";
                var queueName = "fanoutq";

                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange, "fanout", true, false);
                    channel.QueueDeclare(queueName, durable: true);
                    while(true)
                    {
                        
                        channel.QueueBind(queueName, exchange, routingkey);
                        Console.WriteLine("准备接收消息");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (s, e) =>
                         {
                             var routingKey = e.RoutingKey;
                             var body = e.Body;
                             var message = Encoding.UTF8.GetString(body);
                             Console.WriteLine("接收到的消息：'{0}':'{1}'", routingKey, message);
                         };
                        channel.BasicConsume(queueName, false, consumer);
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
