using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.SubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var exchange = "fanoutexchange";
                    var routingkey = "fanoutkey";
                    channel.ExchangeDeclare(exchange, type: "fanout", durable: true, autoDelete: false);
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; //1，不持久化， 2：持久化
                   
                    while (true)
                    {
                        Console.Write("please write your message:");
                        var msg = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(msg);

                        channel.BasicPublish(exchange, routingkey, properties, body);
                        Console.WriteLine("已发送信息：'{0}':'{1}'", routingkey, msg);
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
