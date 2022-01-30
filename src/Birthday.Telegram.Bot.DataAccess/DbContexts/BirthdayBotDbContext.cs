using Birthday.Telegram.Bot.Domain.AggregationModels;
using Microsoft.EntityFrameworkCore;

namespace Birthday.Telegram.Bot.DataAccess.DbContexts
{
    public class BirthdayBotDbContext : DbContext
    {
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<Chat> GroupChats { get; set; }
        public DbSet<ChatChatMember> GroupChatChatMembers { get; set; }

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