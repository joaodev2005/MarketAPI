using FluentMigrator;

namespace MarketAPI.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersion.TABLE_ORDERS, "Creating Orders and OrderItems tables")]
public class Version0000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Orders")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("UserId").AsGuid().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(1)
            .WithColumn("Total").AsDecimal(10, 2).NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithDefaultValue(SystemMethods.CurrentUTCDateTime);

        Create.ForeignKey("FK_Orders_Users")
            .FromTable("Orders").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");

        Create.Table("OrderItems")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("OrderId").AsGuid().NotNullable()
            .WithColumn("ProductId").AsGuid().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal(10, 2).NotNullable();

        Create.ForeignKey("FK_OrderItems_Orders")
            .FromTable("OrderItems").ForeignColumn("OrderId")
            .ToTable("Orders").PrimaryColumn("Id");

        Create.ForeignKey("FK_OrderItems_Products")
            .FromTable("OrderItems").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id");
    }
}