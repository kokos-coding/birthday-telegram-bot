using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Birthday.Telegram.Bot.Controllers
{
    /// <summary>
    /// Main bot controller
    /// </summary>
    [Route("[controller]")]
    public class BotController : Controller
    {
        private readonly IBotMessageService _botMessageService;
        private readonly ILogger<BotController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">ILogger instance</param>
        public BotController(IBotMessageService botMessageService, ILogger<BotController> logger)
        {
            _logger = logger;
            _botMessageService = _botMessageService;
        }

        /// <summary>
        ///     Post method for receive messages from Bot (Using webhook)
        /// </summary>
        /// <param name="update">New Update from bot</param>
        /// <returns>Ok Action or error</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            await _botMessageService.ProcessUpdateAsync(update);
            return Ok();
        }
    }
}