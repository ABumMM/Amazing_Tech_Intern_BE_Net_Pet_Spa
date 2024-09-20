using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Repositories.Context;
using PetSpa.Repositories.UOW;
using PetSpa.Services;
using PetSpa.Services.Service;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetSpa"),
        sqlOptions => sqlOptions.MigrationsAssembly("PetSpa.Repositories"));
});

builder.Services.AddServices();

builder.Services.AddRepositories();

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
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
