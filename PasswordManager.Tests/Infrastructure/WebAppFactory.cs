using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace PasswordManager.Tests.Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>
{
    //Базовая конфигурация
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //TODO тут нужно сделать подмену файла конфигурации
        });
    }
}