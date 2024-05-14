using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Proyecto_U3.Converters;
using U3Api.Models.Entities;
using U3Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionstring = builder.Configuration.GetConnectionString("ApiU3ConnectionString");
builder.Services.AddDbContext<ItesrcneActividadesContext>(x => x.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring)));

builder.Services.AddTransient<Repository<Departamentos>>();
builder.Services.AddTransient<Repository<Actividades>>();


builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.Audience = "pruebaU3";
    x.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("oWgVSB6azKciOUdwnRSMxKPyccYXiVp0qP0svFWemCQRK45kkbf3rqHbykHHYntKYyMxjKFJia9n7ZbKiC380uFSBuSuhzRd8IhY"));
    x.TokenValidationParameters.ValidIssuer = "itesrcU3";

});


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(opt =>
    opt.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

// CORS: Permite solicitudes que vienen desde otro dominio diferente.
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
