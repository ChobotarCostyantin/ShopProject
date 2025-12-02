var builder = DistributedApplication.CreateBuilder(args);

var ordersDb = builder.AddPostgres("orders-db")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("ordersDb");

var orderApi = builder.AddProject<Projects.Orders_API>("orders-api")
    .WithReference(ordersDb)
    .WithHttpEndpoint(port: 5001, name: "orders-http")
    .WaitFor(ordersDb);

var catalogDb = builder.AddPostgres("catalog-db")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("catalogDb");

var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(catalogDb)
    .WithHttpEndpoint(port: 5002, name: "catalog-http")
    .WaitFor(catalogDb);

builder.Build().Run();
