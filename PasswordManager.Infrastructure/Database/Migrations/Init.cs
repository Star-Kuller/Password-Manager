using FluentMigrator;

namespace PasswordManager.Infrastructure.Database.Migrations;

[Migration(2024_03_01_1613)] 
public class Init : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("PasswordHash").AsString(255).NotNullable() 
            .WithColumn("SecretKey").AsString(255).NotNullable();
    }
}