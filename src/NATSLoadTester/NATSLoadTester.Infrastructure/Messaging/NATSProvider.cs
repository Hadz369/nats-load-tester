using NATS.Client.Core;
using NATS.Net;
using NATSLoadTester.Application;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NATSLoadTester.Infrastructure.Messaging
{
    public class NATSProvider : IMessageBusProvider
    {
        private string _natsUrl = "nats://127.0.0.1:4222";

        NatsClient _natsClient;

        #region Singleton Initialisation

        private static IMessageBusProvider? instance = null;
        private static readonly object padlock = new object();

        private NATSProvider() 
        {
            _natsUrl = Environment.GetEnvironmentVariable("NATS_URL") ?? _natsUrl;
            _natsClient = new NatsClient(_natsUrl);
        }

        public static IMessageBusProvider Instance
        {
            get
            {
                lock (padlock)
                {
                    instance ??= new NATSProvider();
                    return instance;
                }
            }
        }

        #endregion Singleton Initialisation
        
        public void Publish(string subject, byte[]? data = null, Dictionary<string, string>? headers = null)
        {
            _natsClient.PublishAsync(subject, data: data, headers: PrepareMessageHeaders(headers));
        }

        public void PublishString(string subject, string message, Dictionary<string, string>? headers = null)
        {
            _natsClient.PublishAsync(subject, data: message, headers: PrepareMessageHeaders(headers));
        }

        public void Subscribe(string subject, Action<IMessageBusMessage> messageReceivedAction, CancellationToken cancellationToken)
        {
            Task task = new Task(async () =>
            {
                await foreach (var msg in _natsClient.SubscribeAsync<object>(subject, cancellationToken: cancellationToken))
                {
                    IMessageBusMessage message = FormatResponseMessage(msg);
                    messageReceivedAction(message);
                }
            });

            task.Start();
        }

        #region Message Helpers

        private IMessageBusMessage FormatResponseMessage<T>(NatsMsg<T> message)
        {
            return new MessageBusMessage(
                message.Subject, 
                message.ReplyTo ?? "", 
                ConvertMessageHeadersToDictionary(message.Headers), 
                message.Data);
        }

        private NatsHeaders? PrepareMessageHeaders(Dictionary<string, string>? headers)
        {
            NatsHeaders? natsHeaders = null;

            if (headers != null)
            {
                natsHeaders = new NatsHeaders();
                foreach (KeyValuePair<string, string> header in headers)
                {
                    natsHeaders.Add(header.Key, header.Value);
                }
            }
            return natsHeaders;
        }

        private Dictionary<string, string>? ConvertMessageHeadersToDictionary(NatsHeaders? headers)
        {
            Dictionary<string, string>? convertedHeaders = null;

            if (headers != null)
            {
                convertedHeaders = new Dictionary<string, string>();
                foreach (var header in headers)
                {
                    convertedHeaders.Add(header.Key, header.Value);
                }
            }
            return convertedHeaders;
        }

        #endregion Message Helpers

        public void Dispose()
        {
            _natsClient.DisposeAsync();
        }
    }
}
