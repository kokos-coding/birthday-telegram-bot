namespace Birthday.Telegram.Bot.Domain.AggregationModels
{
    /// <summary>
    /// Model that represents member of chat
    /// </summary>
    public class ChatMember
    {
        /// <summary>
        /// Telegram user id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Chat member name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Birth Day of member
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// Link to chats for user
        /// </summary>
        public ICollection<ChatChatMember> GroupChatChatMembers { get; set; } = new List<ChatChatMember>();
    }
}