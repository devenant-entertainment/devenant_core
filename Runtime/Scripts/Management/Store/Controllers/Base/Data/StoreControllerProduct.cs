namespace Devenant
{
    public class StoreControllerProduct
    {
        public readonly ProductData product;
        public readonly string price;
        public bool value;

        public StoreControllerProduct(ProductData product, string price)
        {
            this.product = product;
            this.price = price;
        }
    }
}
