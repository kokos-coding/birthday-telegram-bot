namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
    /// <summary>
    /// Model that represent chat information
    /// </summary>
    public class GroupChat
    {
        /// <summary>
        /// Identifier of main chat id
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Discussion chat id
        /// </summary>
        public int DiscussionChatId { get; set; }

        /// <summary>
        /// Link to main chat members
        /// </summary>
        public ICollection<GroupChatChatMember> GroupChatChatMembers { get; set; } = new List<GroupChatChatMember>();
    }
}