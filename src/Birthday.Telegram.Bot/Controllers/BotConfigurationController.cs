using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Birthday.Telegram.Bot.Configurations;
using Birthday.Telegram.Bot.Models;
using Birthday.Telegram.Bot.Models.InputModels;
using DevQuiz.TelegramBot.Models.ApiResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Exceptions;

namespace Birthday.Telegram.Bot.Controllers
{
    /// <summary>
    /// Controller for edit bot configuration
    /// </summary>
    [Route("[controller]")]
    public class BotConfigurationController : Controller
    {
        private readonly BotConfiguration _botConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BotConfigurationController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public BotConfigurationController(ILogger<BotConfigurationController> logger, 
            IOptions<BotConfiguration> botConfigurationOptions,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _botConfiguration = botConfigurationOptions.Value;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Method for setting webhook for current telegram bot
        /// </summary>
        /// <param name="value">InputModel for set webhook</param>
        /// <param name="cancellationToken">Instance of cancellation token</param>
        /// <returns>Status end of operation</returns>
        [HttpPost("setwebhook")]
        public async Task<SetWebHookApiResult> SetWebhookUrlAsync([FromBody] SetWebhookInputModel value, CancellationToken cancellationToken)
        {

            _logger.LogDebug("Creating typed HttpClient for client {clientName}", Constants.TypedHttpClients.TelegramApi.ClientName);
            var client = _httpClientFactory.CreateClient(Constants.TypedHttpClients.TelegramApi.ToString());
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("url", value.WebhookUrl)
            });
            var response = await client.PostAsync($"/bot{_botConfiguration.AccessToken}/setWebhook", requestContent,
                cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var responseWithError = await response.Content.ReadFromJsonAsync<SetWebHookApiResult>(cancellationToken: cancellationToken);
                if (responseWithError == null)
                    throw new SerializationException(
                        $"Exception while serialization error response from SetWebHook Telegram Api to type {nameof(SetWebHookApiResult)}");
                throw new ApiRequestException(responseWithError.Description, responseWithError.Error_code);
            }

            _logger.LogDebug("WebHook was set successfully");
            var responseSuccess = await response.Content.ReadFromJsonAsync<SetWebHookApiResult>(cancellationToken: cancellationToken);
            if (responseSuccess == null)
                throw new SerializationException(
                    $"Exception while serialization success response from SetWebHook Telegram Api to type {nameof(SetWebHookApiResult)}");
            return responseSuccess;
        }
    }
}