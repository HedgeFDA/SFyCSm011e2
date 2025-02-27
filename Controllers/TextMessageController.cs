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
using System.Text.RegularExpressions;

namespace SFyCSm011e2.Controllers;

/// <summary>
/// Класс для обработки текстовых сообщений
/// </summary>
public class TextMessageController
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
    public TextMessageController(ITelegramBotClient telegramClient, IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _memoryStorage  = memoryStorage;
    }

    /// <summary>
    /// Обработчик события передаваемого в контроллер
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="ct">Токен отмены</param>
    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":
                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();

                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($" Подсчёт количества символов в тексте", "количество"),
                        InlineKeyboardButton.WithCallbackData($" Вычисление суммы чисел", "сумма")
                });

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Какую информацию боту нужно возврашать на полученное сообщение?</b> {Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                break;
            default:
                string result = "";

                if (!string.IsNullOrEmpty(message.Text))
                {
                    result = _memoryStorage.GetSession(message.Chat.Id).Mode switch
                    {
                        "количество" => $"Длина сообщения: {message.Text.Length} знаков",
                        "сумма" => $"Сумма чисел: {Regex.Matches(message.Text, "\\b\\d+\\b").Cast<Match>().Select(m => int.Parse(m.Value)).Sum()}",
                        _ => "Не удалось выполнить рассчет"
                    };
                }

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);

                break;
        }
    }
}