using System.ComponentModel.DataAnnotations;

namespace Birthday.Telegram.Bot.Models.InputModels
{
    /// <summary>
    /// Input model for SetWebhook method
    /// </summary>
    public class SetWebhookInputModel
    {
        /// <summary>
        /// Webhook Url
        /// </summary>
        [Required]
        public string WebhookUrl { get; set; }
    }
}