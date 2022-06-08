// See https://aka.ms/new-console-template for more information


using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;

var channel = GrpcChannel.ForAddress("https://localhost:7007");
var client = new Greeter.GreeterClient(channel);

var response = await client.SayHelloAsync(new HelloRequest { Name = "World!" });
Console.WriteLine(response.Message);

Console.WriteLine();

var metadata = new Metadata
{
    { "Guid", Guid.NewGuid().ToString() },
    { "Name", "" },
    { "DOB", "26/05/1998" }
};
var customerClient = new Customer.CustomerClient(channel);
var customer = await customerClient.GetCustomerByIdAsync(new CustomerRequest { Id = 1 }, metadata);
Console.WriteLine($"{customer.FirstName} {customer.LastName}");

Console.WriteLine();
Console.WriteLine("All customers list:");
Console.WriteLine();

try
{
    using (var call = customerClient.GetAllCustomers(new CustomerStreamRequest(),
            deadline: DateTime.UtcNow.AddSeconds(5)))
    {
        while (await call.ResponseStream.MoveNext())
        {
            var currentCustomer = call.ResponseStream.Current;
            Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.Age}");
        }
    }
}
catch(RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
{
    Console.WriteLine(ex.Message);
}

Console.ReadLine();
