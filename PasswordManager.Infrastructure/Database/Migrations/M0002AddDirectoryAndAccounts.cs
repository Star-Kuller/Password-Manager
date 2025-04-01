using FluentMigrator;

namespace PasswordManager.Infrastructure.Database.Migrations;

[Migration(2025_04_01_1422)] 
public class M0002AddDirectoryAndAccounts : AutoReversingMigration
{
    public override void Up()
    {
        Alter.Table("users")
            .AddColumn("root_directory_id").AsInt64().NotNullable();
        
        Create.Table("directories")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("title").AsBinary().NotNullable()
            .WithColumn("parent_id").AsInt64()
            .WithColumn("owner_id").AsInt64().NotNullable();
        
        Create.ForeignKey("FK_users_directories_root")
            .FromTable("users").ForeignColumn("root_directory_id")
            .ToTable("directories").PrimaryColumn("id");
        
        Create.ForeignKey("FK_directories_directories")
            .FromTable("directories").ForeignColumn("parent_id")
            .ToTable("directories").PrimaryColumn("id");
        
        Create.ForeignKey("FK_directories_users")
            .FromTable("directories").ForeignColumn("owner_id")
            .ToTable("users").PrimaryColumn("id");

        Create.Table("accounts")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("url").AsBinary()
            .WithColumn("login").AsBinary()
            .WithColumn("password").AsBinary()
            .WithColumn("directory_id").AsInt64().NotNullable();
        
        Create.ForeignKey("FK_accounts_directories")
            .FromTable("accounts").ForeignColumn("directory_id")
            .ToTable("directories").PrimaryColumn("id");
        
        Create.Index("IX_directories_parent_id").OnTable("directories").OnColumn("parent_id").Ascending();
        Create.Index("IX_directories_owner_id").OnTable("directories").OnColumn("owner_id").Ascending();
        Create.Index("IX_accounts_directory_id").OnTable("accounts").OnColumn("directory_id").Ascending();
    }
}