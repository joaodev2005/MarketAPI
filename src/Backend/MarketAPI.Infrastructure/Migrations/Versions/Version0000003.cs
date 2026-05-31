using FluentMigrator;

namespace MarketAPI.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersion.TABLE_PRODUCTS, "Creating Products table")]
public class Version0000003 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Products")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Description").AsString(1000).NotNullable()
            .WithColumn("Price").AsDecimal(10, 2).NotNullable()
            .WithColumn("Stock").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CategoryId").AsGuid().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithDefaultValue(SystemMethods.CurrentUTCDateTime);

        Create.ForeignKey("FK_Products_Categories")
            .FromTable("Products").ForeignColumn("CategoryId")
            .ToTable("Categories").PrimaryColumn("Id");
    }
}