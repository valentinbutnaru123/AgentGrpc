using Broker.Models;
using Broker.Services.Interfaces;
using Grpc.Core;
using GrpcAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broker.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorageService _connectionStorageService;
        public SubscriberService(IConnectionStorageService connectionStorageService)
        {
            _connectionStorageService = connectionStorageService;
        }
        public override Task<SubscriberReply> Subscribe(SubscriberRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New client trying to subscribe: {request.Address} {request.Topic}");

            try
            {
                var connect = new Connection(request.Address, request.Topic);
                _connectionStorageService.Add(connect);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Could not add the new connection {request.Address} {request}. {e.Message}");
            }

            var connection = new Connection(request.Address, request.Topic);
            _connectionStorageService.Add(connection);

            Console.WriteLine($"New client subscribed: {request.Address} {request.Topic}");

            return Task.FromResult(new SubscriberReply()
            {
                IsSuccess = true
            });
        }
    }
}
