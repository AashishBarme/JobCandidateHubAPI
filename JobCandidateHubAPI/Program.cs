using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Infrastructure.Persistence;
using JobCandidateHubAPI.Infrastructure.Repositories;
using JobCandidateHubAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    var serverVersion = new Version("8.0.23");
    option.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(serverVersion));
});
builder.Services.AddScoped<IJobCandidateService, JobCandidateService>();
builder.Services.AddScoped<IJobCandidateRepository, JobCandidateRepository>();

var app = builder.Build();

// run migration automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

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
