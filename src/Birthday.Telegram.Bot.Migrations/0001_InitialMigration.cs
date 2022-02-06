using FluentMigrator;

namespace Birthday.Telegram.Bot.Migrations;

[Migration(20220302_0001)]
public class InitialMigration : Migration
{
    public override void Down()
    {
        Delete.Table(Constants.TableNames.Chats);

        Delete.Table(Constants.TableNames.ChatMembers);
        
        Delete.Table(Constants.TableNames.ChatsChatMembers);
    }

    public override void Up()
    {
        var chatRawSql = @$"CREATE TABLE IF NOT EXISTS ""{Constants.TableNames.Chats}"" (" + 
        @"""id"" bigserial,
        ""chat_id"" bigint not null,
        ""discussion_chat_id"" bigint null,
        PRIMARY KEY (""id"")
        )";

        var chatMemberRawSql = @$"CREATE TABLE IF NOT EXISTS ""{Constants.TableNames.ChatMembers}"" (" + 
        @"""id"" bigserial,
        ""member_id"" bigint not null, 
        ""username"" varchar(200) null,
        ""birth_day"" timestamptz null,
        PRIMARY KEY (""id"")
        )";

        var chatChatMembersMemberRawSql = @$"CREATE TABLE IF NOT EXISTS ""{Constants.TableNames.ChatsChatMembers}"" (" + 
        @"""chat_id"" bigint," + 
        @"""member_id"" bigint," + 
        $"constraint fk_chat_chatmember foreign key (\"chat_id\") references \"{Constants.TableNames.Chats}\"," +
        $"constraint fk_chatmember_chat foreign key (\"member_id\") references \"{Constants.TableNames.ChatMembers}\"" +
        ")";

        Execute.Sql(chatRawSql);
        Execute.Sql(chatMemberRawSql);
        Execute.Sql(chatChatMembersMemberRawSql);
    }
}
