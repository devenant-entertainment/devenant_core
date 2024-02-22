using UnityEngine;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>
    {
        public static Action<Purchase> onUpdated;

        public Purchase[] purchases { get { return _purchases; } private set { _purchases = value; } }
        private Purchase[] _purchases;

        private StoreController storeController;

        public void Setup(Purchase.Info[] purchases, Action<bool> callback)
        {
            Request.Get(ApplicationManager.instance.config.endpoints.purchaseGet, UserManager.instance.user.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    storeController = new MobileStoreController();

                    storeController.Setup(purchases, (bool success) =>
                    {
                        if (success)
                        {
                            PurchaseResponse responseData = JsonUtility.FromJson<PurchaseResponse>(response.data);

                            StorePurchase[] storePurchases = storeController.GetStorePurchases();

                            this.purchases = new Purchase[purchases.Length];

                            for(int i = 0; i < this.purchases.Length; i++)
                            {
                                bool purchased = false;
                                string price = string.Empty;

                                foreach(PurchaseResponse.Purchase purchase in responseData.purchases)
                                {
                                    if(purchase.id == purchases[i].id)
                                    {
                                        purchased = purchase.value;

                                        break;
                                    }
                                }

                                foreach(StorePurchase storePurchase in storePurchases)
                                {
                                    if(storePurchase.id == purchases[i].id)
                                    {
                                        price = storePurchase.price;

                                        break;
                                    }
                                }

                                this.purchases[i] = new Purchase(purchases[i], price, purchased);
                                this.purchases[i].onUpdated += onUpdated;
                            }

                            callback?.Invoke(true);
                        }
                        else
                        {
                            callback?.Invoke(false);
                        }
                    });
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Purchase(Purchase purchase, Action<bool> callback = null)
        {
            if (storeController == null)
            {
                callback?.Invoke(false);

                return;
            }

            storeController.Purchase(purchase.info.id, (StorePurchaseResponse response) =>
            {
                if(response.success)
                {
                    purchase.Set(true, response.transaction, callback);
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }
    }
}
