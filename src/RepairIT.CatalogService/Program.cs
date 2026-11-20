using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Configuration;
using RepairIT.CatalogService.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));
builder.Services.AddDbContext<CatalogDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("CatalogDb")));
builder.Services.AddScoped<CatalogDbInitializer>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CatalogDbInitializer>().Initialiser();
}
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
