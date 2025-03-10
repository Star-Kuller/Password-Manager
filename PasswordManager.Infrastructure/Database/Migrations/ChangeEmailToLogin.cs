using FluentMigrator;

namespace PasswordManager.Infrastructure.Database.Migrations;

[Migration(2025_03_10_1503)] 
public class ChangeEmailToLogin : AutoReversingMigration
{
    public override void Up()
    {
        Rename.Column("Email").OnTable("Users").To("Login");
    }
}