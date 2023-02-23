using OnlineMedicineDonation.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using OnlineMedicineDonation.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("ConnStringApiData");
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connectionString));


//(options =>options.UseSqlServer("ApiTestData"));

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
     .AllowAnyHeader());
});


var tokenKey = builder.Configuration.GetConnectionString("TokenKey");// Configuration.GetValue<string>("TokenKey");
var key = Encoding.ASCII.GetBytes(tokenKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddSingleton<ICustomAuthenticationManager>(new JWTAuthenticationManager(tokenKey));

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1",
    new() { Title = "API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseRouting();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllerRoute(
    name: "default",
    pattern: "{area=account}/{controller=Home}/{action=index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




