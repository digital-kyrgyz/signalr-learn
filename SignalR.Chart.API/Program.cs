using Microsoft.EntityFrameworkCore;
using SignalR.Chart.API.Hubs;
using SignalR.Chart.API.Persistance;
using SignalR.Chart.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Chart.API"));
});
builder.Services.AddScoped<CovidService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapHub<CovidHub>("/CovidHub");
app.Run();
