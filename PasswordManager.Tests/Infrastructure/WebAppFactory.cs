using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace PasswordManager.Tests.Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>
{
    //Базовая конфигурация
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Очищаем существующие источники конфигурации
            config.Sources.Clear();

            // Добавляем наш тестовый файл конфигурации
            var testConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.IntegrationTests.json");
            config.AddJsonFile(testConfigPath, optional: false, reloadOnChange: true);

            // Если нужно, можно добавить переменные окружения или другие источники
            config.AddEnvironmentVariables();
        });
        
        builder.ConfigureTestServices(services =>
        {
            //Тут место для моков внешних сервисов
        });
    }
}