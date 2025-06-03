using InventoryBackend.Context;
using InventoryBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//register the table contexts(userAccount)
builder.Services.AddDbContext<userAccountContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));
//register the table cotexts (folders)
builder.Services.AddDbContext<foldersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));
//register the cosmos client (DI)
builder.Services.AddSingleton<CosmosClient>(sp =>
    new CosmosClient(
        builder.Configuration["azurecosmos:uri"],
        builder.Configuration["azurecosmos:accKey"]
    )
);
//register the createInventory service
builder.Services.AddSingleton<createInventory>(obj =>
{
    var client = obj.GetRequiredService<CosmosClient>();
    var configObj = obj.GetRequiredService<IConfiguration>();
    return new createInventory(client, configObj);
});
//register the updateInventory service
builder.Services.AddSingleton<updateInventoryService>(obj =>
{
    var client = obj.GetRequiredService<CosmosClient>();
    var configObj = obj.GetRequiredService<IConfiguration>();
    return new updateInventoryService(client, configObj);
});
//register the removenventory service
builder.Services.AddSingleton<removeInventory>(obj =>
{
    var client = obj.GetRequiredService<CosmosClient>();
    var configObj = obj.GetRequiredService<IConfiguration>();
    return new removeInventory(client, configObj);
});
/*
 builder.Services.AddDbContext<userAccountContext>
 --Registers AppDbContext with dependency injection (DI) in the application.
 --Allows controllers and services to request AppDbContext without manually instantiating it.

(options =>
    options.UseSqlServer(...);
 --Configures AppDbContext to use SQL Server as the database provider. Uses the UseSqlServer method from Microsoft.EntityFrameworkCore.SqlServer

 builder.Configuration.GetConnectionString("AzureSqlConnection"))
  --Retrieves the connection string named "AzureSqlConnection" from the appsettings.json file.
 */



builder.Services.AddSingleton<tokenProvider>();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//securing the endpoints
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Secret"]!)),
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Correct order:
app.UseHttpsRedirection();
//securing endpoints
app.UseAuthentication();  // Must come first
app.UseAuthorization();
// securing endpoints
app.MapControllers();
app.Run();
