using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ThunderServer.API.Configurations;
using ThunderServer.API.Services;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infratructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ThunderServer.API", Version = "v1" });
	c.EnableAnnotations();
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddLogging(builder => builder.AddConsole());

builder.Services.AddOptions<StorageVolumesConfiguration>().Bind(builder.Configuration.GetSection("Storage-Volumes")).ValidateOnStart();

builder.Services.AddDbContext<ThunderServerContext>(options =>
{
	var x = builder.Configuration.GetConnectionString("PostgresConnection");

	options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddHealthChecks().AddNpgSql("PostgresConnection");

builder.Services.AddTransient<IFileManager, FileManager>();

//builder.Services.AddHostedService<FilesMonitorWithWatcher>();
builder.Services.AddHostedService<FilesMonitorWithProvider>();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
