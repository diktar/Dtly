var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
App.DependencyInjection.AddApplication(builder.Services, builder.Configuration);
Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);

// builder.Services.AddScoped<IEventRepository, EventRepository>();
// builder.Services.AddScoped<IEventService, EventService>();

// Add controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Doctorly Calendar API", Version = "v1" });
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Doctorly Calendar API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// hack needed for tests
public partial class Program { }