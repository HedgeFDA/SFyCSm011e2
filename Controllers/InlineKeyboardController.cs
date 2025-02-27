using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using SFyCSm011e2.Configuration;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using SFyCSm011e2.Services;

namespace SFyCSm011e2.Controllers;

/// <summary>
/// Класс для обработки нажатия кнопок
/// </summary>
public class InlineKeyboardController
{
    /// <summary>
    /// Интерфейс для доступа к Telegram Bot API
    /// </summary>
    private readonly ITelegramBotClient _telegramClient;

    /// <summary>
    /// Интерфейс для доступа к сведениям пользовательской сессии
    /// </summary>
    private readonly IStorage _memoryStorage;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="telegramClient">Интерфейс для доступа к API</param>
    /// <param name="memoryStorage">Интерфейс для к сведениям пользовательской сессии</param>
    public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _memoryStorage  = memoryStorage;
    }

    /// <summary>
    /// Обработчик события передаваемого в контроллер
    /// </summary>
    /// <param name="callbackQuery">Выбранный вариант</param>
    /// <param name="ct">Токен отмены</param>
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        if (callbackQuery?.Data == null)
            return;

        // Обновление пользовательской сессии новыми данными
        _memoryStorage.GetSession(callbackQuery.From.Id).Mode = callbackQuery.Data;

        // Генерим информационное сообщение
        string mode = callbackQuery.Data switch
        {
            "количество" => "подсчёт количества символов в тексте",
            "сумма" => "вычисление суммы чисел",
            _ => String.Empty
        };

        // Отправляем в ответ уведомление о выборе
        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
            $"<b>Режим обработки сообщений:\n{mode}.</b>\nМожно поменять в главном меню.", 
            cancellationToken: ct, parseMode: ParseMode.Html);
    }
}