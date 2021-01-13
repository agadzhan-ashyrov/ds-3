using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NATS.Client;
using StackExchange.Redis;

namespace BackendApi.Services
{
    public class JobService : Job.JobBase
    {
        private readonly static IConnection connection = new ConnectionFactory().CreateConnection("nats://127.0.0.1");
        private readonly static GreeterService greeter = new GreeterService();

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            string id = Guid.NewGuid().ToString();

            ConnectionMultiplexer.Connect("localhost").GetDatabase().StringSet(id, request.Description);

            greeter.Run(connection, id);

            var resp = new RegisterResponse
            {
                Id = id
            };

            return Task.FromResult(resp);
        }
    }
}