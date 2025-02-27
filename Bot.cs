using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using SFyCSm011e2.Controllers;

namespace SFyCSm011e2;

/// <summary>
/// Класс для реализации основной логики работы приложения, через данный класс бот будет общаться с API.
/// </summary>
internal class Bot : BackgroundService
{
    /// <summary>
    /// Интерфейс для доступа к Telegram Bot API
    /// </summary>
    private readonly ITelegramBotClient _telegramClient;

    /// <summary>
    /// Контроллер текстовых сообщений
    /// </summary>
    private readonly TextMessageController _textMessageController;

    /// <summary>
    /// Контроллер нажатия кнопок
    /// </summary>
    private readonly InlineKeyboardController _inlineKeyboardController;

    /// <summary>
    /// Контроллер прочих сообщений
    /// </summary>
    private readonly DefaultMessageController _defaultMessageController;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="telegramClient">Интерфейс для доступа к API</param>
    public Bot(
        ITelegramBotClient          telegramClient,
        TextMessageController       textMessageController,
        InlineKeyboardController    inlineKeyboardController,
        DefaultMessageController    defaultMessageController)
    {
        _telegramClient             = telegramClient;
        _textMessageController      = textMessageController;
        _inlineKeyboardController   = inlineKeyboardController;
        _defaultMessageController   = defaultMessageController;
    }

    /// <summary>
    /// Реализация запуска постоянного режима работы бота
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен.");
    }

    /// <summary>
    /// Обаботчик событий (любых действий пользователя в Telegram)
    /// </summary>
    async Task HandleUpdateAsync(ITelegramBotClient telegramClient, Update update, CancellationToken cancellationToken)
    {
        //  Обрабатываем нажатия на кнопки из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);

            return;
        }

        // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    await _textMessageController.Handle(update.Message, cancellationToken);

                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);

                    return;
            }
        }
    }

    /// <summary>
    /// Обаботчик ошибок
    /// </summary>
    Task HandleErrorAsync(ITelegramBotClient telegramClient, Exception exception, CancellationToken cancellationToken)
    {
        // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        // Выводим в консоль информацию об ошибке
        Console.WriteLine(errorMessage);

        // Задержка перед повторным подключением
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");

        Thread.Sleep(10000);

        return Task.CompletedTask;
    }
}