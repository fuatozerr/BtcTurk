using BtcTurk.Context;
using BtcTurk.Extensions;
using BtcTurk.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BtcTurk Mulakat Calismasi",
        Contact = new OpenApiContact
        {
            Name = "Faruk Fuat Ozer",
            Url = new Uri("https://linkedin.com/in/fuatozerr"),
            Email = "fuatozerr23@gmail.com"
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


builder.Services.AddInfrasturctureRegistration(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
using (var scope = app.Services.CreateScope()) //dockerda ayağa kalınca otomatik db olustursun
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<BtcTurkDbContext>();
    dbContext.Database.Migrate();
}
app.UseMiddleware<ErrorMiddleware>();
app.UseHttpsRedirection();
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseAuthorization();

app.MapControllers();

app.Run();
