using Microsoft.EntityFrameworkCore;
using Refresca.Api.Application.Interfaces;
using Refresca.Api.Infrastructure.Persistence;
using Refresca.Api.Infrastructure.Repositories;
using Refresca.Api.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Refresca")
    ?? "Data Source=refresca.db";

builder.Services.AddDbContext<RefrescaDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRefrescaService, RefrescaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("App", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<Refresca.Api.Infrastructure.Middleware.ExceptionHandlingMiddleware>();
app.UseCors("App");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RefrescaDbContext>();
    await db.Database.EnsureCreatedAsync();
    await SeedData.InitializeAsync(db);
}

app.Run();

public partial class Program { }
