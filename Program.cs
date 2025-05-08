using flight_tracker;
using flight_tracker.Data;
using flight_tracker.Service;
using flight_tracker.Service.ServiceInterface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {   //allow react to use localhost
    options.AddPolicy("AllowReactApp", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();  //Need these two for DB
builder.Services.AddScoped<IFlightData, FlightData>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
