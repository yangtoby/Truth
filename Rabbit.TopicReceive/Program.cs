using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.TopicReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var exchange = "topicexchange";
                var routingKey = "topickey.AA.*";
                var queueName = "topicq1";

                channel.ExchangeDeclare(exchange, "topic", true, false);
                channel.QueueDeclare(queueName, true, autoDelete: false);

                while (true)
                {
                    channel.QueueBind(queueName, exchange, routingKey);
                    Console.WriteLine("准备接收消息：");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) =>
                     {
                         var rkey = e.RoutingKey;
                         var body = e.Body;
                         var message = Encoding.UTF8.GetString(body);
                         Console.WriteLine("接收到的消息： '{0}':'{1}'", rkey, message);
                     };
                    channel.BasicConsume(queueName, false, consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
