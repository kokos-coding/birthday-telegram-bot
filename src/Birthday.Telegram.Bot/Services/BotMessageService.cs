using System.Linq;
using System.Threading.Tasks;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Birthday.Telegram.Bot.Services;

/// <inheritdoc cref="IBotMessageService" />
public class BotMessageService : IBotMessageService
{
    private readonly ILogger<BotMessageService> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mediator"> Mediator</param>
    /// <param name="logger"> Logge r</param>
    public BotMessageService(
        //IBotService botService,
        //IUserService<UserDto, Guid> userService,
        IMediator mediator,
        ILogger<BotMessageService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc cref="ProcessUpdateAsync" />
    public async Task ProcessUpdateAsync(Update update)
    {
#if !(DEBUG)
            if (update.Type != UpdateType.Message)
                return;
#endif
        var message = update.Message;

        _logger.LogInformation("Received Message from {messageChatId}", message.Chat.Id);

        var firstEntityType = message
            .Entities
            .FirstOrDefault()?
            .Type;

        if (firstEntityType == MessageEntityType.BotCommand)
            switch (message.Text)
            {
                case Constants.BotCommands.Start:
                    await _mediator.Send(new StartCommand(message));
                    break;
                default:
                    _logger.LogError($"Unknown bot command \"{{messageText}}\"", message.Text);
                    break;
            }
    }
}