using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFyCSm011e2;
using SFyCSm011e2.Configuration;
using SFyCSm011e2.Controllers;
using SFyCSm011e2.Services;
using Telegram.Bot;

namespace SFyCSm011e2;

public class Program
{
    /// <summary>
    /// Регистрация/подключение всех необходимых сервисов и компонент для приложения (Dependency Injection)
    /// </summary>
    static void ConfigureServices(IServiceCollection services)
    {
        // Инициализируем и регистрируем конфигурацию приложения
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        // Подключаем контроллеры сообщений и кнопок
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<DefaultMessageController>();

        // Регистрируем хранилище сессий
        services.AddSingleton<IStorage, MemoryStorage>();

        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }

    /// <summary>
    /// Построение конфигурации приложения
    /// </summary>
    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "7355291958:AAFatoENe97YIUibZ7Ejip2OwMYNpKPXhiI"
        };
    }

    /// <summary>
    /// Главная точка входа приложения
    /// </summary>
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем

        Console.WriteLine("Сервис запущен");

        // Запускаем сервис
        await host.RunAsync();

        Console.WriteLine("Сервис остановлен");
    }
}
