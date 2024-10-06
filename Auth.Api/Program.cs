using Auth.Api.Middlewares;
using Auth.Api.Validators;
using Auth.Business;
using Auth.Business.Interfaces;
using Auth.Common.Helper;
using Auth.Common.Mappings;
using Auth.Common.Models.Request;
using Auth.Common.Settings;
using Auth.Data.Context;
using Auth.Data.UoW;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<RegisterRequestModel>, RegisterRequestModelValidator>();
builder.Services.AddScoped<IValidator<LoginRequestModel>, LoginRequestModelValidator>();

builder.Services.AddDbContext<AuthDbContext>(item => item.UseSqlServer(builder.Configuration.GetConnectionString("AuthDatabase")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<UserMapping>();
builder.Services.AddSingleton<PasswordPolicyHelper>();

builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtTokenSettings>>().Value);

builder.Services.Configure<PasswordPolicySettings>(builder.Configuration.GetSection("PasswordPolicySettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<PasswordPolicySettings>>().Value);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtTokenSettings:Key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ValidateTokenMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
