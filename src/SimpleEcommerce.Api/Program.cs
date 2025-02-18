using SimpleEcommerce.Api.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using SimpleEcommerce.Api.Security;
using IdentityModel;
using System.Security.Claims;
using SimpleEcommerce.Api.Infrastructure;
using SimpleEcommerce.Api.Extensions;
using SimpleEcommerce.Api.Services.Jwt;
using SimpleEcommerce.Api.Services.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<EcommerceDbContext>(cfg =>
{
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("Default"), opt =>
    {
        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
{
    opt.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
    opt.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
    opt.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Roles;
    opt.ClaimsIdentity.EmailClaimType = JwtClaimTypes.Email;
}).AddEntityFrameworkStores<EcommerceDbContext>()
.AddClaimsPrincipalFactory<AppClaimPricnibalFactory>();


var jwtConfig = new JwtConfiguration();

builder.Configuration.Bind(JwtConfiguration.CONFIG_KEY, jwtConfig);

builder.Services.AddSingleton(jwtConfig);

var s3StorageConfig = new S3StorageConfiguration();

builder.Configuration.Bind(S3StorageConfiguration.CONFIG_KEY, s3StorageConfig);

builder.Services.AddSingleton(s3StorageConfig);

builder.Services.AddTransient<ICurrentUser, CurrentUser>();

builder.Services.AddTransient<ICurrentPrincibalAccessor, HttpContextCurrentPrincibalAccessor>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddResponseFactory(Assembly.GetExecutingAssembly());

builder.Services.AddApplicaitonService(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true, 
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer")!,
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience")!,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey")!))
        };

    });

var app = builder.Build();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(bld => bld.AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader()
  );
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
