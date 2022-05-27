// See https://aka.ms/new-console-template for more information


using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;

var channel = GrpcChannel.ForAddress("https://localhost:7007");
var client = new Greeter.GreeterClient(channel);

var response = await client.SayHelloAsync(new HelloRequest { Name = "World!" });
Console.WriteLine(response.Message);

Console.WriteLine();

var customerClient = new Customer.CustomerClient(channel);
var customer = await customerClient.GetCustomerByIdAsync(new CustomerRequest { Id = 1 });
Console.WriteLine($"{customer.FirstName} {customer.LastName}");

Console.WriteLine();
Console.WriteLine("All customers list:");
Console.WriteLine();

using(var call = customerClient.GetAllCustomers(new CustomerStreamRequest()))
{
    while (await call.ResponseStream.MoveNext())
    {
        var currentCustomer = call.ResponseStream.Current;
        Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.Age}");
    }
}

Console.ReadLine();
