using Birthday.Telegram.Bot.Domain.AggregationModels;
using Microsoft.EntityFrameworkCore;

namespace Birthday.Telegram.Bot.DataAccess.DbContexts
{
    /// <summary>
    /// Context for manage EF Db connection
    /// </summary>
    public class BirthdayBotDbContext : DbContext
    {
        /// <summary>
        /// Chat members table
        /// </summary>
        public DbSet<ChatMember> ChatMembers { get; set; }

        /// <summary>
        /// Chats table
        /// </summary>
        public DbSet<Chat> Chats { get; set; }

        /// <summary>
        /// Table for link table chat and chatmembers
        /// </summary>
        /// <value></value>
        public DbSet<ChatChatMember> ChatChatMembers { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">DbContext options</param>
        public BirthdayBotDbContext(DbContextOptions<BirthdayBotDbContext> options) : base(options)
        {

        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ChatMember>()
                .ToTable("chat_member");

            modelBuilder
                .Entity<Chat>()
                .ToTable("group_chat");

            modelBuilder
                .Entity<ChatChatMember>()
                .ToTable("group_chat_chat_member");
        }
    }
}