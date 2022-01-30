using Birthday.Telegram.Bot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Services;

public class BotChatMemberProcessor : IBotChatMemberProcessor
{
    private readonly ITelegramBotClient _telegramBotClient;

    public BotChatMemberProcessor(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task ProcessAsync(ChatMemberUpdated action, CancellationToken cancellationToken)
    {
        var processStatus = action.NewChatMember.Status;

        var processUpdate = processStatus switch 
        {
            ChatMemberStatus.Member => AddBotToChatAction(action.Chat, cancellationToken),
            ChatMemberStatus.Left => RemoveBotFromChat(action.Chat, cancellationToken)
        };

        await processUpdate;
    }

    private async Task AddBotToChatAction(Chat chatInfo, CancellationToken cancellationToken)
    {
        // Спросить текущую инфу о боте
        var botInfo = await _telegramBotClient.GetMeAsync(cancellationToken);
        // Отправить приветственное сообщение
        await SendHelloMessageToChat(chatInfo, botInfo, cancellationToken);
        // Записать в БД данные о чате
        // Связать данные о чате с пользователями
    }

    private Task RemoveBotFromChat(Chat chatInfo, CancellationToken cancellationToken)
    {
        // При удалении 

        return Task.CompletedTask;
    }

    private async Task SendHelloMessageToChat(Chat chatInfo, User botInfo, CancellationToken cancellationToken)
    {
        var helloMessageString = @$"Привет всем\! 
Я Ваш помощник для отслеживания дней рождений\.
Чтобы начать вам помогать мне нужна некая информация
Для этого\, пожалуйста\, переидете по [этой](https://t.me/{botInfo.Username}?start=-{chatInfo.Id}) ссылочке и ответьте на парочку вопросов

Я не читаю ваши переписки, мне это совершенно не нужно\.";


        await _telegramBotClient.SendTextMessageAsync(chatInfo.Id, 
                        helloMessageString, 
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken);
    }
}
