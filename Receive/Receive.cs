// See https://aka.ms/new-console-template for more information


using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost", DispatchConsumersAsync = true };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("hello", false, false, false, null);
Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Message Get" + ea.DeliveryTag);
    Console.WriteLine($" [x] Received {message}");
    await Task.Yield();
};

channel.BasicConsume(queue: "hello",
    autoAck: true,
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();