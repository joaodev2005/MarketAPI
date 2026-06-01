using FluentMigrator;

namespace MarketAPI.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersion.TABLE_CARTS, "Creating Carts and CartItems tables")]
public class Version0000004 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Carts")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("UserId").AsGuid().NotNullable();

        Create.ForeignKey("FK_Carts_Users")
            .FromTable("Carts").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id");

        Create.Table("CartItems")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CartId").AsGuid().NotNullable()
            .WithColumn("ProductId").AsGuid().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable();

        Create.ForeignKey("FK_CartItems_Carts")
            .FromTable("CartItems").ForeignColumn("CartId")
            .ToTable("Carts").PrimaryColumn("Id");

        Create.ForeignKey("FK_CartItems_Products")
            .FromTable("CartItems").ForeignColumn("ProductId")
            .ToTable("Products").PrimaryColumn("Id");
    }
}