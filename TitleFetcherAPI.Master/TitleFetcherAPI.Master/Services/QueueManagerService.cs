using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitleFetcherAPI.Master.BL;
using TitleFetcherAPI.Master.Models;
using TitleFetcherAPI.Master.Models.ResponseModels;

namespace TitleFetcherAPI.Master.Services
{
    public class QueueManagerService : IQueueManagerService
    {
        private const string requestQueue = "title-request-queue";
        private const string responseQueue = "title-response-queue";

        private readonly IConfiguration _config;
        private readonly IConnection Conn;

        /// <summary>
        /// Initialize the service, while subscribing to the response-queue
        /// </summary>
        /// <param name="config"></param>
        public QueueManagerService(IConfiguration config)
        {
            _config = config;

            var connectionEndpoint = _config.GetSection("Queue").GetValue<string>("ConnectionFactoryEndpoint");
            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionEndpoint)
            };
            Conn = factory.CreateConnection();
            var channel = Conn.CreateModel();
            channel.QueueDeclare(responseQueue, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                HandleMessage(body);
            };

            channel.BasicConsume(responseQueue, true, consumer);
        }


        /// <summary>
        /// Iterate the urls in the request and send a message to the request-queue for the slaves to handle
        /// </summary>
        /// <param name="urls"></param>
        public void SendMessage(List<string> urls)
        {            
            var channel = Conn.CreateModel();
            channel.QueueDeclare(requestQueue, true, false, false, null);

            foreach(var url in urls)
            {
                var message = new { Url = url };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", requestQueue, null, body);
            }            
        }

        private void HandleMessage(byte[] body)
        {
            var message = JsonConvert.DeserializeObject<TitleResponse>(Encoding.UTF8.GetString(body));

            UrlTitleStorage.AddUrlTitle(new UrlTitle { DateAdded = DateTime.Now, Title = message.Title, Url = message.Url });
        }
    }
}
