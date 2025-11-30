var builder = DistributedApplication.CreateBuilder(args);

var ordersDb = builder.AddPostgres("orders-db")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("ordersDb");

var orderApi = builder.AddProject<Projects.Orders_API>("orders-api")
    .WithReference(ordersDb)
    .WithHttpEndpoint(port: 5001, name: "orders-http")
    .WaitFor(ordersDb);

builder.Build().Run();
