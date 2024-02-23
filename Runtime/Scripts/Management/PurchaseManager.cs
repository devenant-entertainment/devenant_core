using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>
    {
        public static Action<Purchase> onPurchased;

        public Purchase[] purchases { get { return _purchases; } private set { _purchases = value; } }
        private Purchase[] _purchases;

        private StoreController storeController;

        public void Setup(PurchaseData[] purchases, Action<bool> callback)
        {
            Request.Get(ApplicationManager.instance.backend.purchaseGet, UserManager.instance.user.token, (Request.Response response) =>
            {
                if(response.success)
                {
#if UNITY_EDITOR
                    storeController = new EditorStoreController();
#elif UNITY_ANDROID || UNITY_IOS
                    storeController = new MobileStoreController();
#else
                    storeController = new EditorStoreController();
#endif

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
                                    if(purchase.id == purchases[i].name)
                                    {
                                        purchased = purchase.value;

                                        break;
                                    }
                                }

                                foreach(StorePurchase storePurchase in storePurchases)
                                {
                                    if(storePurchase.id == purchases[i].name)
                                    {
                                        price = storePurchase.price;

                                        break;
                                    }
                                }

                                this.purchases[i] = new Purchase(purchases[i].name, purchases[i].type, price, purchased);
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

        public void Purchase(string id, Action<bool> callback = null)
        {
            if(storeController == null)
            {
                callback?.Invoke(false);

                return;
            }

            foreach(Purchase purchase in purchases)
            {
                if(purchase.id == id)
                {
                    storeController.Purchase(purchase.id, (StorePurchaseResponse response) =>
                    {
                        if(response.success)
                        {
                            if(purchase.purchased == true)
                            {
                                callback?.Invoke(false);

                                return;
                            }

                            purchase.purchased = true;

                            onPurchased?.Invoke(purchase);

                            Dictionary<string, string> formFields = new Dictionary<string, string>()
                            {
                                {"id", purchase.id },
                                {"purchased", purchase.purchased.ToString() },
                                {"platform", UnityEngine.Application.platform.ToString() },
                                {"transaction", response.transaction }
                            };

                            Request.Post(ApplicationManager.instance.backend.purchaseSet, formFields, UserManager.instance.user.token, (Request.Response response) =>
                            {
                                callback?.Invoke(response.success);
                            });
                        }
                        else
                        {
                            callback?.Invoke(false);
                        }
                    });
                    break;
                }
            }
        }
    }
}
