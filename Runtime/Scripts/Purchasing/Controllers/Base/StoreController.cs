namespace Devenant
{
    public abstract class StoreController
    {
        public abstract void Setup(Product[] products, Action<StoreProduct[]> callback);
        public abstract void Purchase(StoreProduct product, Action<Purchase> callback);
    }
}