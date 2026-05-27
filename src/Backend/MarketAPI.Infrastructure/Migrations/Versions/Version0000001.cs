using FluentMigrator;

namespace MarketAPI.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersion.TABLE_USERS, "Creating Users table")]
public class Version0000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Role").AsInt32().NotNullable().WithDefaultValue(1)
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime);
    }
}