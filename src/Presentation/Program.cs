using Azure.Storage.Blobs;
using Core.Interface.Repositories;
using Core.Interface.Services;
using Core.Interfaces.Repositories;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Presentation.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
var blobConnectionString = builder.Configuration.GetConnectionString("AZURE_PLANT_STORAGE");

builder.Services.AddSingleton(provider =>
{
    var containerName = builder.Configuration["Storage:CONTAINER_NAME"];
    var client = new BlobContainerClient(blobConnectionString, containerName);
    client.CreateIfNotExists();
    return client;
});

builder.Services.AddDbContext<PlantDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IWateringLogRepository, WateringLogRepository>();

builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IWateringLogService, WateringLogService>();

builder.Services.AddScoped<IPlantImageService, AzurePlantImageService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PlantDbContext>();

    await SeedData.InitializeAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
