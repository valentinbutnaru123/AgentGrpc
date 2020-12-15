using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Grpc.Net.Client;
using GrpcAgent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(EndpointsConstants.SubscriberAddres)
                .Build();

            host.Start();

            Subscribe(host);

            Console.WriteLine("Press enter to exist");
            Console.ReadLine();
        }

        private static void Subscribe(IWebHost host)
        {
            var channel = GrpcChannel.ForAddress(EndpointsConstants.BrokerAddres);
            var client = new Subscriber.SubscriberClient(channel);

            Console.Write("Enter the topic: ");
            var topic = Console.ReadLine().ToLower();

            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Subscriber listening at: {address}");

            var request = new SubscriberRequest() { Topic = topic, Address = address };

            try
            {
                var reply = client.Subscribe(request);
                Console.WriteLine($"Subscriber repl: {reply.IsSuccess}");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error subscribing: {e.Message}");
            }
        }
    }
}
