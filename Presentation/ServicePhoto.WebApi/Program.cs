using ServicePhoto.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();
app.ConfigureMiddleware(builder.Environment);

app.Run();
