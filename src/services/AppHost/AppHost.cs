var builder = DistributedApplication.CreateBuilder(args);

// --- Databases ---
var ordersDb = builder.AddPostgres("orders-db")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("ordersDb");

var catalogDb = builder.AddPostgres("catalog-db")
    .WithDataVolume()
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust")
    .AddDatabase("catalogDb");

var socialAndReviewsDb = builder.AddMongoDB("social-and-reviews-db")
    .WithDataVolume()
    .AddDatabase("socialAndReviewsDb");

// --- Microservices ---
var orderApi = builder.AddProject<Projects.Orders_API>("orders-api")
    .WithReference(ordersDb)
    .WaitFor(ordersDb);

var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(catalogDb)
    .WaitFor(catalogDb);

var socialAndReviewsApi = builder.AddProject<Projects.SocialAndReviews_API>("social-and-reviews-api")
    .WithReference(socialAndReviewsDb)
    .WaitFor(socialAndReviewsDb);

// --- Aggregator ---
var aggregator = builder.AddProject<Projects.Aggregator>("aggregator")
    .WithReference(orderApi)
    .WithReference(catalogApi)
    .WithReference(socialAndReviewsApi);

// --- API Gateway ---
var apiGateway = builder.AddProject<Projects.ApiGateway>("api-gateway")
    .WithReference(orderApi)
    .WithReference(catalogApi)
    .WithReference(socialAndReviewsApi)
    .WithReference(aggregator)
    .WithHttpEndpoint(port: 5000, name: "gateway-http");

builder.Build().Run();