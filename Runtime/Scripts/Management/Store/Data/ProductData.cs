namespace Devenant
{
    public enum ProductType
    {
        Consumable,
        Unlockable,
        Subscription
    }

    public class ProductData : AssetData
    {
        public readonly ProductType type;

        public ProductData(ProductDataAsset product) : base(product) 
        {
            type = product.type;
        }
    }
}
