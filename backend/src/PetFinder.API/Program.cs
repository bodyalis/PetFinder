using PetFinder.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Configure().Build();
Serilog.Debugging.SelfLog.Enable(Console.Error);
await app.ConfigureAsync();
app.Run();

public partial class Program;