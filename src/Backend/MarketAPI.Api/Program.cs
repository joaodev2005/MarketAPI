using MarketAPI.Application;
using MarketAPI.Infrastructure;
using MarketAPI.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ExecuteMigrations();

app.Run();

void ExecuteMigrations()
{ 
    using  var scope = app.Services.CreateScope();
    
    DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}
