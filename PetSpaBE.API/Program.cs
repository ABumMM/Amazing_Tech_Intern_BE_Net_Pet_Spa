<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Repositories.Context;
using PetSpa.Repositories.UOW;
using PetSpa.Services;
using PetSpa.Services.Service;
=======
﻿using Microsoft.EntityFrameworkCore;
using PetSpa.Repositories.Context;
using PetSpa.Services;
>>>>>>> 39145a9053671ca5fa5ac234fa1a5ae4c7496cac

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetSpa"),
        sqlOptions => sqlOptions.MigrationsAssembly("PetSpa.Repositories"));
});
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories();


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

app.Run();
