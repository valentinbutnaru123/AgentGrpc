using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class Connection
    {
        public Connection(string address, string topic)
        {
            Addres = address;
            Topic = topic;
            Channel = GrpcChannel.ForAddress(address);
        }
        public string Addres { get; }
        public string Topic { get; }

        public GrpcChannel Channel { get; }
    }
}
