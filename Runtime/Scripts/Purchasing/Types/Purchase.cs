namespace Devenant
{
    public struct Purchase
    {
        public bool success;
        public StoreProduct product;
        public string transaction;
        public string receipt;

        public Purchase(bool success, StoreProduct product, string transaction, string receipt)
        {
            this.success = success;
            this.product = product;
            this.transaction = transaction;
            this.receipt = receipt;
        }
    }
}
