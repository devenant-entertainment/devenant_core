namespace Devenant
{
    public enum ProductType
    {
        Consumable,
        Unlockable,
        Subscription
    }

    public class Product : Asset
    {
        public readonly ProductType type;

        public Product(SOProduct product) : base(product) 
        {
            type = product.type;
        }
    }
}
