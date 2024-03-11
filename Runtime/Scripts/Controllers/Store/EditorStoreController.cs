namespace Devenant
{
    public class EditorStoreController : StoreController
    {
        private SOPurchase[] purchases;

        public override void Setup(SOPurchase[] purchases, Action<bool> callback)
        {
            this.purchases = purchases;

            callback?.Invoke(true);
        }

        public override void Purchase(string id, Action<StorePurchaseResponse> callback)
        {
            callback?.Invoke(new StorePurchaseResponse(true, "EditorTransaction"));
        }

        public override StorePurchase[] GetStorePurchases()
        {
            StorePurchase[] storePurchases = new StorePurchase[purchases.Length];

            for(int i = 0; i < storePurchases.Length; i ++)
            {
                storePurchases[i] = new StorePurchase(purchases[i].name, "0,00$", false);
            }

            return storePurchases;
        }
    }
}
