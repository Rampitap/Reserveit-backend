using Reserveit.API.Extensions;
using Reserveit.API.Middlewares;
using Reserveit.Application.Extensions;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Extensions;
using Reserveit.Infrastructure.Seeders;
using Serilog;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrasrtucture(builder.Configuration);


//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


var app = builder.Build();
app.LogStartupBanner();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IReservationSeeder>();

await seeder.Seed();

app.UseMiddleware<ErrorHadlingMiddleware>();
app.UseMiddleware<RequestLoggerMiddlware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();



app.UseAuthorization();

app.MapControllers();

app.Run();
