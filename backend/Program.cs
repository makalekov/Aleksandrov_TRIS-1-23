using backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITicketService, TicketService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok("ok"));
app.MapGet("/info", () => Results.Ok(new
{
    app = "CableTickets API",
    environment = app.Environment.EnvironmentName,
    lab = "ЛР7–8: REST API заявок на повреждение кабеля"
}));

app.MapControllers();
app.Run();
