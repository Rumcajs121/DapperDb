using FluentValidation;
using MinnimalWebApi_net9.Endpoints;
using MinnimalWebApi_net9.Models;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<ISqlDataAcces, SqlDataAcces>();
builder.Services.AddSingleton<IToDoListData, ToDoListData>();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(ToDoValidator));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();
app.MapAssignmentEndpoints();
app.Run();

public partial class Program{}