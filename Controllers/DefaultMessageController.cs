using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using SFyCSm011e2.Configuration;

namespace SFyCSm011e2.Controllers;

/// <summary>
/// Класс для обработки сообщений по умолчанию (для всех "прочих" сообщений, у которых нет своего контроллера обработчика)
/// </summary>
public class DefaultMessageController
{
    /// <summary>
    /// Интерфейс для доступа к Telegram Bot API
    /// </summary>
    private readonly ITelegramBotClient _telegramClient;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="telegramClient">Интерфейс для доступа к API</param>
    public DefaultMessageController(ITelegramBotClient telegramClient)
    {
        _telegramClient = telegramClient;
    }

    /// <summary>
    /// Обработчик события передаваемого в контроллер
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="ct">Токен отмены</param>
    public async Task Handle(Message message, CancellationToken ct)
    {
        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", cancellationToken: ct);
    }
}
