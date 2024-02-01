using Core.Constants;
using E_Commerce_Platform.Utli;
using Infrastructure;
using Infrastructure.Seeder;
using Majbora.Util;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(SecretConstant.DefaultConnectionString,
                                                      sqlDbOptions=> sqlDbOptions.MigrationsAssembly(SecretConstant.MigrationTargetProject)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.InjectApplicationRepos();

builder.Services.AddSwaggerDocumantation();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<DataContext>();

          context?.Database.Migrate();
    await DataSeed.SaveData(context);
    
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
