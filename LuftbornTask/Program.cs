using LuftbornTask;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                    options.UseSqlServer(connectionString)
                                                           .EnableDetailedErrors()
                                                           .EnableSensitiveDataLogging()
                                                           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddControllers();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
       builder => builder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod());
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.UseCors("AllowAnyOrigin");

app.Run();
