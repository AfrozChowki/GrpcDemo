using Grpc.Core;

namespace GrpcService.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerInfoModel> GetCustomerById(CustomerRequest request, ServerCallContext context)
        {
            CustomerInfoModel customerInfo = new CustomerInfoModel();

            if(request.Id == 1)
            {
                customerInfo.FirstName = "Firoz";
                customerInfo.LastName = "Chowki";
            }
            else if(request.Id == 2)
            {
                customerInfo.FirstName = "Manish";
                customerInfo.LastName = "Malgonde";
            }
            else if (request.Id == 3)
            {
                customerInfo.FirstName = "Sanket";
                customerInfo.LastName = "Khedkar";
            }
            else
            {
                customerInfo.FirstName = "Jatin";
                customerInfo.LastName = "Bhate";
            }

            return Task.FromResult(customerInfo);
        }

        public override async Task GetAllCustomers(
            CustomerStreamRequest request, 
            IServerStreamWriter<CustomerInfoModel> responseStream, 
            ServerCallContext context)
        {
            List<CustomerInfoModel> customers = GetCustomers();

            foreach(var cust in customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(cust);
            }
        }

        private List<CustomerInfoModel> GetCustomers()
        {
            return new List<CustomerInfoModel>() { 
                new CustomerInfoModel {
                    FirstName = "Firoz",
                    LastName = "Chowki",
                    Age = 22,
                    IsAlive = true
                },
                new CustomerInfoModel
                {
                    FirstName = "Jatin",
                    LastName = "Bhate",
                    Age = 23,
                    IsAlive = true
                },
                new CustomerInfoModel
                {
                    FirstName = "Test",
                    LastName = "User",
                    Age = 25,
                    IsAlive = false
                }
            };
        }
    }
}
