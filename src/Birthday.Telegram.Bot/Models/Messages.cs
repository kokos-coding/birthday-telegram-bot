using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Models;

/// <summary>
/// Static class with prebuild messages
/// </summary>
public static class Messages
{
    /// <summary>
    /// REgex spec symbols
    /// </summary>
    /// <returns></returns>
    private static Regex SpecialSymbols => new(@"[_*\[\]\(\)~`>#\+-=|{}.,!]", RegexOptions.Compiled);

    private static string ToMarkdownV2(string input) =>
        SpecialSymbols.Replace(input, "\\$&");

    /// <summary>
    /// Parse mode for all messages
    /// </summary>
    public static ParseMode ParseMode => ParseMode.MarkdownV2;

    public static string ChatNameForBirthday = "birthday_chat";
    public static string ChatNameForBirthdayMarkdown => ChatNameForBirthday.Replace("_", @"\_");

    /// <summary>
    /// Hello message
    /// </summary>
    /// <param name="userName">Message for whom</param>
    public static string HelloMessage(string userName) => @$"Приветствую тебя, *{ToMarkdownV2(userName)}*\!😎

Я бот \- помощник по организации поздравлений\.
Моя задача отслеживать дни рождения участников чата\, и приглашать всех кроме именинника в отдельный чат для обсуждения подарка\.
А так же поздравлять именинника в основном чате\, для запуска волны поздравлений\!

Чтобы я начал работать\, мне нужна твоя помощь\. 
Алгоритм не сложный\, но нужно сделать все по пунктам\.

➡️ Добавь меня в ваш основной чат\. Я пришлю приветственное сообщение в котором будет ссылонька\, по ней должны перейти все участники\, и шепнуть свой даты рождения\.
➡️ Создай отдельный чат с названием ```{ToMarkdownV2(ChatNameForBirthdayMarkdown)}``` и добавь меня туда\. В него я буду приглашать пользователей для обсуждения праздника\.
➡️ Перешли в этот чат любое сообщение из основного чата\. Это нужно чтобы связать его с основным\.

На этом все\!

Примечание\. Я не читаю вашу переписку из чатов\, мне это совершенно не интересно\. Для того чтобы начать общение со мной в чате для этого есть специальные команды\.";

    public static string MessageForGetBirthday(string userName, string fromChatName) => @$"Привет *{ToMarkdownV2(userName)}*\.
Чтобы пользователи чата *{ToMarkdownV2(fromChatName)}* смогли поздравлять тебя мне нужно шепнуть дату твоего дня рождения\.";
}
