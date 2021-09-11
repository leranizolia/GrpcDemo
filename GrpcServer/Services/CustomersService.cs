using Grpc.Core;
using GrpcServer.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }

            return Task.FromResult(output);        
        }

        public override async Task GetNewCustomers(
            NewCustomerRequest request,
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>()
            {
                new CustomerModel
                {
                    FirstName = "Tim",
                    LastName = "Corey",
                    EmailAddress = "tim@iamtimcorrey.com",
                    Age = 41,
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName = "Sue",
                    LastName = "Storm",
                    EmailAddress = "sue@stormy.net",
                    Age = 28,
                    IsActive = false
                },
                new CustomerModel
                {
                    FirstName = "Bilbo",
                    LastName = "Baggins",
                    EmailAddress = "bilbo@baggins.com",
                    Age = 75,
                    IsActive = false
                }
            };

            //just returning all customers doesn`t work
            //because of stream in proto
            foreach (var customer in customers)
            {
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
