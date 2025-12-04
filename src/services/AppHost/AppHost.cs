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

var socialAndReviewsDb = builder.AddMongoDB("social-and-reviews-db")
    .WithDataVolume()
    .AddDatabase("socialAndReviewsDb");

var socialAndReviewsApi = builder.AddProject<Projects.SocialAndReviews_API>("social-and-reviews-api")
    .WithReference(socialAndReviewsDb)
    .WithHttpEndpoint(port: 5003, name: "social-and-reviews-http")
    .WaitFor(socialAndReviewsDb);

builder.Build().Run();
