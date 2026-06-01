namespace MarketAPI.Infrastructure.Migrations;

internal abstract class DatabaseVersion
{
    internal const int TABLE_USERS = 1;
    internal const int TABLE_CATEGORIES = 2;
    internal const int TABLE_PRODUCTS = 3;
    internal const int TABLE_CARTS = 4;
}