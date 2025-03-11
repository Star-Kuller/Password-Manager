using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using PasswordManager.API;

namespace PasswordManager.Tests.Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>
{
    //Базовая конфигурация
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
        builder.ConfigureTestServices(services =>
        {
            //Тут место для моков внешних сервисов
        });
    }
}