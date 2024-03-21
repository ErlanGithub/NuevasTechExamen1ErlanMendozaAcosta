using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductoProveedor.Contratos.Repositorio;
using ProductoProveedor.Implementacion;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(/*worker=>worker.UseNewtonsoftJson()*/)
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IProductoRepositorio, ProductoImplementacion>();
        services.AddScoped<IProveedorRepositorio, ProveeedorImplementacion>();

    })
    .Build();

host.Run();
