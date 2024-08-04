namespace Devenant
{
    public struct PurchaseResponse
    {
        public bool success;
        public StoreControllerProduct product;
        public string transaction;
        public string receipt;

        public PurchaseResponse(bool success, StoreControllerProduct product, string transaction, string receipt)
        {
            this.success = success;
            this.product = product;
            this.transaction = transaction;
            this.receipt = receipt;
        }
    }
}
