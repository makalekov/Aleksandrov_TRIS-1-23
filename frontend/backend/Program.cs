using backend.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ITicketService, TicketService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/info", () => Results.Ok(new
{
    app = "CableTickets API",
    environment = app.Environment.EnvironmentName,
    lab = "ЛР7: каркас ASP.NET Core Web API"
}));

app.Run();
