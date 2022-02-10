using System.Text.RegularExpressions;

namespace Birthday.Telegram.Bot.Helpers;

/// <summary>
/// Вспомогательный класс для работы с текстом. <br/>
/// Для преобразования в другие форматы текста
/// </summary>
public static class TextHelper
{
    /// <summary>
    /// Регулярное выражение для поиска специальных символов
    /// </summary>
    private static Regex SpecialSymbols => new(@"[_*\[\]\(\)~`>#\+-=|{}.,!]", RegexOptions.Compiled);

    /// <summary>
    /// Перевод текста в MarkdownV2
    /// </summary>
    /// <param name="input">Входная строка</param>
    /// <returns>Текст в формате MarkdownV2</returns>
    internal static string ToMarkdownV2(this string input) =>
        SpecialSymbols.Replace(input, "\\$&");
}
