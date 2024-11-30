using System;
using System.Text;
using System.Collections.Generic;
using NATS.Client;
using NATS.Net;
using NATS.Client.Core;

namespace NATSLoadTester.Infrastructure
{
    public class NATSProvider : IMessageBusProvider
    {
        private string _natsUrl = "nats://127.0.0.1:4222";

        NatsClient _natsClient;

        public NATSProvider() 
        {
            _natsUrl = Environment.GetEnvironmentVariable("NATS_URL") ?? _natsUrl;
            _natsClient = new NatsClient(_natsUrl);
        }

        public void Publish(string subject, byte[]? data = null, Dictionary<string, string>? headers = null)
        {
            _natsClient.PublishAsync(subject, data: data, headers: PrepareMessageHeaders(headers));
        }

        public void PublishString(string subject, string message, Dictionary<string, string>? headers = null)
        {
            _natsClient.PublishAsync(subject, data: message, headers: PrepareMessageHeaders(headers));
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
        public void Dispose()
        {
            _natsClient.DisposeAsync();
        }
    }
}
