using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region [ Core Services ]

var sql = builder.AddSqlServer("constructionsite-sql")
    .WithDataVolume("constructionsite-sql-volume");

var authenticationDb = sql.AddDatabase("authentication-db", "Authentication_Db");

var activityDb = sql.AddDatabase("activity-db", "Activity_Db");

var cache = builder.AddRedis("cache");

#endregion

#region [ APIs ]

var authenticationApiService = builder.AddProject<ConstructionSite_Services_Authentication_API>("authentication-api")
    .WithReference(authenticationDb)
    .WaitFor(authenticationDb)
    .WithEnvironment("OTEL_LOGS_EXPORTER", "console");

var activityApiService = builder.AddProject<Projects.ConstructionSite_Services_Activity_API>("activity-api")
    .WithReference(activityDb)
    .WaitFor(activityDb)
    .WithEnvironment("OTEL_LOGS_EXPORTER", "console");


#endregion


builder.AddProject<Projects.ConstructionSite_Web>("constructionsite-web")
    .WithExternalHttpEndpoints()
    .WithReference(authenticationApiService)
    .WaitFor(authenticationApiService)
    .WithReference(activityApiService)
    .WaitFor(activityApiService);


builder.Build().Run();
