using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using TitleFetcher.Slave.Models.RequestModels;
using TitleFetcher.Slave.TitleFetcher.Abstraction;

namespace TitleFetcher.Slave.QueueManager
{
    public static class QueueManagerIns
    {
        private const string requestQueue = "title-request-queue";
        private const string responseQueue = "title-response-queue";

        private static readonly IConnection Conn;
        public static bool init = false;


        /// <summary>
        /// Initialize the queue manager while subscribing to the request-queue
        /// </summary>
        static QueueManagerIns()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(AppConfig.ConnectionFactoryEndpoint)
            };
            Conn = factory.CreateConnection();
            var channel = Conn.CreateModel();
            channel.QueueDeclare(requestQueue, durable: true, exclusive: false, autoDelete: false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                HandleMessage(body);
            };

            channel.BasicConsume(requestQueue, true, consumer);
        }

        /// <summary>
        /// Send a message to the response-queue for the master to handle
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="siteTitle"></param>
        public static void SendMessage(string siteUrl, string siteTitle)
        {
            var message = new { Url = siteUrl, Title = siteTitle };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var channel = Conn.CreateModel();
            channel.QueueDeclare(responseQueue, true, false, false, null);
            channel.BasicPublish("", responseQueue, null, body);
        }

        public async static void HandleMessage(byte[] body)
        {
            var message = JsonConvert.DeserializeObject<TitleRequest>(Encoding.UTF8.GetString(body));
            TitleFetcherService titleFetcher = new TitleFetcherService();

            var title = await titleFetcher.GetSiteTitle(message.Url);

            SendMessage(message.Url, title);
        }
    }
}
