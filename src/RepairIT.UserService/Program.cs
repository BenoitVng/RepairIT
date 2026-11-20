using Microsoft.EntityFrameworkCore;
using RepairIT.UserService.Configuration;
using RepairIT.UserService.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("UserDb")));
builder.Services.AddScoped<UserDbInitializer>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<UserDbInitializer>().Initialiser();
}
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
