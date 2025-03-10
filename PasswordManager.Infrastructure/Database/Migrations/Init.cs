using FluentMigrator;

namespace PasswordManager.Infrastructure.Database.Migrations;

[Migration(2025_03_01_1613)] 
public class Init : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Email").AsBinary().NotNullable().Unique()
            .WithColumn("PasswordHash").AsBinary().NotNullable() 
            .WithColumn("SecretKey").AsBinary().NotNullable();
    }
}