namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
    /// <summary>
    /// Model that links Chat and chat member
    /// </summary>
    public class GroupChatChatMember
    {
        /// <summary>
        /// Identifier of chat
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Identifier of member
        /// </summary>
        public int MemberId { get; set; }
    }
}