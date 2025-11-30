var builder = DistributedApplication.CreateBuilder(args);

var orderDb = builder.AddPostgres("ordersDb")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("orderDb");

var orderApi = builder.AddProject<Projects.Orders_API>("orders-api")
    .WithReference(orderDb)
    .WithHttpEndpoint(port: 5001, name: "orders-http")
    .WaitFor(orderDb);

builder.Build().Run();
