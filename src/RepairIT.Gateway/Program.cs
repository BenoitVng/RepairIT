var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.MapGet("/", () => "RepairIT Gateway");
app.Run();