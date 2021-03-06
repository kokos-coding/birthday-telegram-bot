using Birthday.Telegram.Bot.Helpers;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Models;

/// <summary>
/// Static class with prebuild messages
/// </summary>
public static class Messages
{
    /// <summary>
    /// Parse mode for all messages
    /// </summary>
    public static ParseMode ParseMode => ParseMode.MarkdownV2;
    private static string ConvertToDefaultParseMode(string input) => input.ToMarkdownV2();

    /// <summary>
    /// Значение чата ChatNameForBirthday в кодировке MarkdownV2
    /// </summary>
    private static string ChatNameForBirthdayMarkdown => ConvertToDefaultParseMode(Constants.ChatNameForBirthday);

    /// <summary>
    /// Hello message
    /// </summary>
    /// <param name="userName">Message for whom</param>
    public static string HelloMessage(string userName) => @$"Приветствую тебя, *{ConvertToDefaultParseMode(userName)}*\!😎

Я бот \- помощник по организации поздравлений\.
Моя задача отслеживать дни рождения участников чата\, и приглашать всех кроме именинника в отдельный чат для обсуждения подарка\.
А так же поздравлять именинника в основном чате\, для запуска волны поздравлений\!

Чтобы я начал работать\, мне нужна твоя помощь\. 
Алгоритм не сложный\, но нужно сделать все по пунктам\.

➡️ Добавь меня в ваш основной чат\. Я пришлю приветственное сообщение в котором будет ссылонька\, по ней должны перейти все участники\, и шепнуть свой даты рождения\.
➡️ Создай отдельный чат с названием ```{ConvertToDefaultParseMode(ChatNameForBirthdayMarkdown)}``` и добавь меня туда\. В него я буду приглашать пользователей для обсуждения праздника\.
➡️ Перешли в этот чат любое сообщение из основного чата\. Это нужно чтобы связать его с основным\.

На этом все\!

Примечание\. Я не читаю вашу переписку из чатов\, мне это совершенно не интересно\. Для того чтобы начать общение со мной в чате для этого есть специальные команды\.";

    /// <summary>
    /// Приветственное сообщение когда бот хочет получить дату рождения пользователя
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="fromChatName">Откуда пришел пользователь</param>
    /// <returns>Сообщение</returns>
    public static string MessageForGetBirthdayDate(string userName, string fromChatName) => @$"Привет *{ConvertToDefaultParseMode(userName)}*\.
Чтобы пользователи чата *{ConvertToDefaultParseMode(fromChatName)}* смогли поздравлять тебя мне нужно шепнуть дату твоего дня рождения\.";

    /// <summary>
    /// Приветственное сообщение от бота, когда он входит в главный чат
    /// </summary>
    /// <param name="botName">Название бота</param>
    /// <param name="mainChatId">Идентификатор главного чата</param>
    /// <returns>Приветственное сообщение</returns>
    public static string HelloMessageFromBotToMainChat(string botName, string mainChatId) => @$"Привет всем\! 
Я Ваш помощник для отслеживания дней рождений\.
Чтобы начать вам помогать мне нужна некая информация
Для этого\, пожалуйста\, перейдите по [этой](https://t.me/{botName}?start={mainChatId}) ссылочке и ответьте на парочку вопросов

Я не читаю ваши переписки, мне это совершенно не нужно\.";

    /// <summary>
    /// Приветственное сообщение от бота, когда новый пользователь входит в чат
    /// </summary>
    /// <param name="newUserName">Имя нового пользователя чата</param>
    /// <param name="botName">Название бота</param>
    /// <param name="mainChatId">Идентификатор главного чата</param>
    /// <returns>Приветственное сообщение</returns>
    public static string HelloMessageForNewChatUser(string newUserName, string botName, string mainChatId) => @$"Привет *{newUserName}*\! 
Твои друзья попросили меня следить за тем, у кого когда день рождение\.
Чтобы я смог оповещать и о твоем дне рождения перейди\, пожалуйста\, по [этой](https://t.me/{botName}?start={mainChatId}) ссылочке и ответь на парочку вопросов";

    /// <summary>
    /// Сообщение которое отправляется после того, как пользователь вводит свое день рождение
    /// </summary>
    /// <returns>Сообщение</returns>
    public static string MessageAfterSaveBirthdayDate() => @$"✔️ Ваш день рождения внесен в календарь\!

Теперь я смогу\:
— напоминать о ближайших днях рождения 🥳
— приглашать в чаты для обсуждения ближайших ДР 🤫
— поздравлять именинников в группе в 9 утра 🎊";

    /// <summary>
    /// Типизированные сообщения об ошибках
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Сообщение не удалось распознать
        /// </summary>
        public const string MessageCouldNotRecognized = @"Я не распознал выше сообщение\. Введите /start \.";

        /// <summary>
        /// Пустое сообщение
        /// </summary>
        public const string EmptyMessage = @"Полученное сообщение пустое\.\.\. Я не понимаю что вы хотите мне сказать\.";

        /// <summary>
        /// Ошибка сервера
        /// </summary>
        public const string ServerError = @"Ошибка сервера\.";

        /// <summary>
        /// Данный юзер не принадлежит чату
        /// </summary>
        public const string UserNotInChat = @"Извините\, но вы не состоите в том чате\, откуда получили ссылку\. Для продождения нажмите /start \.";
    }
}
