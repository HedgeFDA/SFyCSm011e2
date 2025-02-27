using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFyCSm011e2.Models;

/// <summary>
/// Класс для хранения сведений о пользовательской сессии
/// </summary>
public class Session
{
    /// <summary>
    /// Режим ответов на входящие сообщения от пользователя
    /// "количество"    - подсчёт количества символов в тексте
    /// "сумма"         - вычисление суммы чисел
    /// </summary>
    public string? Mode { get; set; }
}