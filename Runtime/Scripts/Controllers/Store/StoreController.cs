namespace Devenant
{
    public struct StorePurchaseResponse
    {
        public readonly bool success;
        public readonly string transaction;

        public StorePurchaseResponse(bool success, string transaction)
        {
            this.success = success;
            this.transaction = transaction;
        }
    }

    public struct StorePurchase
    {
        public readonly string id;
        public readonly string price;
        public readonly bool value;

        public StorePurchase(string id, string price, bool value)
        {
            this.id = id;
            this.price = price;
            this.value = value;
        }
    }

    public abstract class StoreController
    {
        public abstract void Setup(SOPurchase[] purchases, Action<bool> callback);
        public abstract void Purchase(string id, Action<StorePurchaseResponse> callback);
        public abstract StorePurchase[] GetStorePurchases();
    }
}
