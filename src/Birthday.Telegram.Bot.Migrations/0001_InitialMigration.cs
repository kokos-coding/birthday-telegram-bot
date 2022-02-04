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
        Create.Table(Constants.TableNames.Chats);

        Create.Table(Constants.TableNames.ChatMembers);

        Create.Table(Constants.TableNames.ChatsChatMembers);
    }
}
