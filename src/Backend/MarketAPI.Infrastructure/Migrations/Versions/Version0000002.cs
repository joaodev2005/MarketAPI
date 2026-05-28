using FluentMigrator;

namespace MarketAPI.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersion.TABLE_CATEGORIES, "Creating Categories table")]
public class Version0000002 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(100).NotNullable();
    }
}