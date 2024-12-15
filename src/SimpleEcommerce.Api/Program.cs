using SimpleEcommerce.Api.EntityFramework;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Autofac.Extensions.DependencyInjection;
using SimpleEcommerce.Api.Services;

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

var s3StorageConfig = new S3StorageConfiguration();

builder.Configuration.Bind(S3StorageConfiguration.CONFIG_KEY, s3StorageConfig);

builder.Services.AddSingleton(s3StorageConfig);

builder.Services.AddTransient<S3StorageService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
