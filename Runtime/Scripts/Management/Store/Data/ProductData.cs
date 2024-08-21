namespace Devenant
{
    public enum ProductType
    {
        Consumable,
        Unlockable,
        Subscription
    }

    public class ProductData : EntityData
    {
        public readonly ProductType type;

        public ProductData(ProductAsset product) : base(product) 
        {
            type = product.type;
        }
    }
}
