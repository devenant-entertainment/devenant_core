namespace Devenant
{
    public struct PurchaseResponse
    {
        public bool success;
        public StoreControllerProduct product;
        public string transaction;

        public PurchaseResponse(bool success, StoreControllerProduct product, string transaction)
        {
            this.success = success;
            this.product = product;
            this.transaction = transaction;
        }
    }
}
