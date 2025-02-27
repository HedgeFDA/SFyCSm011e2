using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFyCSm011e2.Configuration;

/// <summary>
/// Класс реализующий конфиуграцию запускаемого приложения
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Токен бота Telegram 
    /// </summary>
    public string? BotToken { get; set; }
}
