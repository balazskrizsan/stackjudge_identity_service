using WeatherService;
using WeatherService.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("IdentityService", client =>
{
    var baseUrl = builder.Configuration.GetValue<string>("Authorization:IdentityServiceUrl");
    client.BaseAddress = new Uri(baseUrl);
});


builder.Services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseIdentityServerAuthorization(builder.Configuration.GetValue<string>("Authorization:IdentityServiceCredentials"));

app.MapWeatherForecastApi();
app.Run();
