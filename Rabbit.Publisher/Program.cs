using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Rabbit.Publisher
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
                var routingKey = "topickey.AA.A1";
                var queueName = "topicq";

                channel.ExchangeDeclare(exchange, "topic", true);
                //channel.QueueDeclare(queueName, true);
                //channel.QueueBind(queueName, exchange, routingKey);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                while (true)
                {
                    Console.Write("请输入要发送的消息:");
                  //  var keyMsg = Console.ReadLine();
                   
                    var msg = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(msg);

                    channel.BasicPublish(exchange, routingKey, properties, body);
                    Console.WriteLine("已发送信息：'{0}':'{1}'", routingKey, msg);
                }
            }
        }
    }
}
