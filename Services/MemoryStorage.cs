using SFyCSm011e2.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFyCSm011e2.Services;

/// <summary>
/// Класс реализующий интерфейс пользовательских сессий
/// </summary>
public class MemoryStorage : IStorage
{
    /// <summary>
    /// Хранилище сессий
    /// </summary>
    private readonly ConcurrentDictionary<long, Session> _sessions;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    public MemoryStorage()
    {
        _sessions = new ConcurrentDictionary<long, Session>();
    }

    /// <summary>
    /// Получение сессии пользователя по идентификатору
    /// </summary>
    /// <param name="chatId">Идентификатор пользовательской сессии сведения которой нужно получить</param>
    public Session GetSession(long chatId)
    {
        if (_sessions.ContainsKey(chatId))
            return _sessions[chatId];

        var newSession = new Session() { Mode = "количество" };

        _sessions.TryAdd(chatId, newSession);

        return newSession;
    }
}