using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using WebApplication1.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(); 
builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddSingleton<ErrorResponseHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 

app.MapControllers();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}