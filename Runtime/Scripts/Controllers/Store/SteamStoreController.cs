namespace Devenant
{
    public class SteamStoreController : StoreController
    {
        private PurchaseData[] purchases;

        public override void Setup(PurchaseData[] purchases, Action<bool> callback)
        {
            //TODO

            this.purchases = purchases;

            callback?.Invoke(true);
        }

        public override void Purchase(string id, Action<StorePurchaseResponse> callback)
        {
            //TODO

            callback?.Invoke(new StorePurchaseResponse(true, "EditorTransaction"));
        }

        public override StorePurchase[] GetStorePurchases()
        {
            //TODO

            StorePurchase[] storePurchases = new StorePurchase[purchases.Length];

            for(int i = 0; i < storePurchases.Length; i++)
            {
                storePurchases[i] = new StorePurchase(purchases[i].name, "0,00$", false);
            }

            return storePurchases;
        }
    }
}
