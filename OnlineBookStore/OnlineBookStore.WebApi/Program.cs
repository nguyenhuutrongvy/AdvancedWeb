using OnlineBookStore.WebApi.Endpoints;
using OnlineBookStore.WebApi.Extensions;
using OnlineBookStore.WebApi.Mapsters;
using OnlineBookStore.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureServices()
        .ConfigureCors()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}

var app = builder.Build();
{
    app.SetupRequestPipeline();

    app.MapAuthorEndpoints();
    app.MapCategoryEndpoints();
    app.MapBookEndpoints();
    app.MapAdminEndpoints();
}

app.Run();