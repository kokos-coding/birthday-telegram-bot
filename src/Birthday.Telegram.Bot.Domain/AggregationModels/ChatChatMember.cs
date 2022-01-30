namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
    /// <summary>
    /// Model that links Chat and chat member
    /// </summary>
    public class ChatChatMember
    {
        /// <summary>
        /// Identifier of chat
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Identifier of member
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Chat link
        /// </summary>
        public Chat? Chat { get; set; }

        /// <summary>
        /// Chat member link
        /// </summary>
        public ChatMember? ChatMember { get; set; }
    }
}