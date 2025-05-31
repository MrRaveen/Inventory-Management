using InventoryBackend.Context;
using InventoryBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//register the table contexts
builder.Services.AddDbContext<userAccountContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//secure endpoints
app.UseAuthentication();
app.UseAuthorization();
app.Run();
