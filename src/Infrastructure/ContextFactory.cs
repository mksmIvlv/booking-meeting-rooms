using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

/// <summary>
/// Класс для миграции
/// </summary>
public class ContextFactory: IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();
             
        // Получить конфигурацию из файла appsettings.json
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath("D:/Personal/Homework/C#/My project/Project_6/Presentation/");
        builder.AddJsonFile("appsettings.json");
        IConfigurationRoot config = builder.Build();
 
        // Получить строку подключения из файла appsettings.json
        string connectionString = config.GetSection("InfrastructureSettings")["ConnectionStringDb"];
        optionsBuilder.UseNpgsql(connectionString);
        return new Context(optionsBuilder.Options);
    }
}