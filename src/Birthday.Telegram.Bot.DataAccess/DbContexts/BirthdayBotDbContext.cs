using Birthday.Telegram.Bot.Domain.AggregationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
#pragma warning disable CS8618

namespace Birthday.Telegram.Bot.DataAccess.DbContexts;

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
            .Entity<ChatMember>(opt =>
            {
                opt.ToTable("chat_members");
                opt.HasKey(p => p.Id);
                opt.Property(p => p.Id).HasColumnName("id");
                opt.Property(p => p.MemberId).HasColumnName("member_id");
                opt.Property(p => p.Username).HasColumnName("username");
                opt.Property(p => p.BirthDay).HasColumnName("birth_day");
            });

        modelBuilder
            .Entity<Chat>(opt =>
            {
                opt.ToTable("chats");
                opt.HasKey(p => p.Id);
                opt.Property(p => p.Id).HasColumnName("id");
                opt.Property(p => p.ChatId).HasColumnName("chat_id");
                opt.Property(p => p.DiscussionChatId).HasColumnName("discussion_chat_id");
            });

        modelBuilder
            .Entity<ChatChatMember>(opt =>
            {
                opt.ToTable("chats_chat_members");
                opt.HasKey(p => new { p.ChatId, p.MemberId });
                opt.HasOne(p => p.Chat)
                    .WithMany(p => p.GroupChatChatMembers)
                    .HasForeignKey(p => p.ChatId);
                opt.HasOne(p => p.ChatMember)
                    .WithMany(p => p.GroupChatChatMembers)
                    .HasForeignKey(p => p.MemberId);

                opt.Property(p => p.ChatId).HasColumnName("chat_id");
                opt.Property(p => p.MemberId).HasColumnName("member_id");
            });

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }
}
