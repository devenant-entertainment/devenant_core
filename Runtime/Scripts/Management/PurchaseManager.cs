using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>
    {
        public static Action<Purchase> onPurchased;

        public PurchaseDataContent purchases = new PurchaseDataContent();

        private StoreController storeController;

        public void Setup(Action<bool> callback)
        {
            purchases.Setup((Purchase[] purchases) =>
            {
                Dictionary<string, string> formFields = new Dictionary<string, string>
                {
                    { "token", UserManager.instance.user.token }
                };

                Request.Post(ApplicationManager.instance.backend.purchaseGet, formFields, (Request.Response response) =>
                {
                    if(response.success)
                    {
#if UNITY_EDITOR
                        storeController = new EditorStoreController();
#elif UNITY_ANDROID || UNITY_IOS
                        storeController = new MobileStoreController();
#else
                        storeController = new SteamStoreController();
#endif
                        storeController.Setup(this.purchases.Get(), (bool success) =>
                        {
                            if(success)
                            {
                                PurchaseResponse responseData = JsonUtility.FromJson<PurchaseResponse>(response.data);

                                StorePurchase[] storePurchases = storeController.GetStorePurchases();

                                foreach(StorePurchase storePurchase in storePurchases)
                                {
                                    this.purchases.Get(storePurchase.id).price = storePurchase.price;
                                }

                                foreach(PurchaseResponse.Purchase purchase in responseData.purchases)
                                {
                                    this.purchases.Get(purchase.name).purchased = purchase.value;
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
            });
        }

        public void Purchase(string name, Action<bool> callback = null)
        {
            if(storeController == null)
            {
                callback?.Invoke(false);

                return;
            }

            Purchase purchase = purchases.Get(name);

            if(purchase == null)
            {
                callback?.Invoke(false);

                return;
            }

            if(purchase.purchased == true)
            {
                callback?.Invoke(false);

                return;
            }

            storeController.Purchase(purchase.name, (StorePurchaseResponse response) =>
            {
                if(response.success)
                {
                    purchase.purchased = true;

                    onPurchased?.Invoke(purchase);

                    Dictionary<string, string> formFields = new Dictionary<string, string>()
                    {
                        { "token", UserManager.instance.user.token },
                        { "name", purchase.name },
                        { "transaction", response.transaction },
                        { "platform", UnityEngine.Application.platform.ToString() },
                        { "value", purchase.purchased.ToString() }
                    };

                    Request.Post(ApplicationManager.instance.backend.purchaseSet, formFields, (Request.Response response) =>
                    {
                        callback?.Invoke(response.success);
                    });
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }
    }
}
