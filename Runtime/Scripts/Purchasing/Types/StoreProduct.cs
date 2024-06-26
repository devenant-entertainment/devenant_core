namespace Devenant
{
    public class StoreProduct
    {
        public readonly Product product;
        public readonly string price;
        public bool value;

        public StoreProduct(Product product, string price)
        {
            this.product = product;
            this.price = price;
        }
    }
}
