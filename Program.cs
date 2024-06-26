using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using SimpleSelfEmploy.Data;
using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Data;
using SimpleSelfEmployApi.Services;
using System.Text.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

var kvUri = builder.Configuration["KeyVault:KeyVaultUrl"];
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
var mongoDbSettingsResponse = await client.GetSecretAsync($"{env}-{appName}-MongoDbSettings");
var mongoDbSettings = JsonSerializer.Deserialize<MongoDbSettings>(mongoDbSettingsResponse.Value.Value);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure Database
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoDbSettings.ConnectionString;
    options.DatabaseName = mongoDbSettings.DatabaseName;
});
builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

//Configure Services
builder.Services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
builder.Services.AddScoped(typeof(JobRepository));
builder.Services.AddScoped(typeof(IJobService), typeof(JobService));
builder.Services.AddScoped(typeof(PaymentRepository));
builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowProductionReactApp",
        builder =>
        {
            builder.WithOrigins("https://simple-self-employ.vercel.app")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowReactApp");
}
else
{
    app.UseHttpsRedirection();
    app.UseCors("AllowProductionReactApp");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
