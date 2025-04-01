using FluentMigrator;

namespace PasswordManager.Infrastructure.Database.Migrations;

[Migration(2025_03_01_1613)] 
public class M0001Init : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("login").AsBinary().NotNullable().Unique()
            .WithColumn("password_hash").AsBinary().NotNullable() 
            .WithColumn("secret_key").AsBinary().NotNullable();
    }
}