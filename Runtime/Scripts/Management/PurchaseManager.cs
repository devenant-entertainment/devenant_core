using Codice.Client.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>
    {
        public static Action<Purchase> onPurchased;

        [System.Serializable]
        private class Response
        {
            public Purchase[] purchases;

            [System.Serializable]
            public class Purchase
            {
                public string id;
                public string type;
                public bool value;
            }
        }

        public class Purchase
        {
            public class Info
            {
                public enum Type
                {
                    Consumable,
                    Non_Consumable,
                    Subscription
                }

                public readonly string id;
                public readonly Type type;

                public Info(string id, Type type)
                {
                    this.id = id;
                    this.type = type;
                }
            }

            public readonly Info info;

            public bool purchased { get { return _purchased; } private set { _purchased = value; } }
            private bool _purchased;

            public Purchase(Info info, bool purchased)
            {
                this.info = info;
                this.purchased = purchased;
            }

            public void Set(Action<bool> callback = null)
            {
                if(!purchased)
                {
                    callback?.Invoke(false);

                    return;
                }

                purchased = true;

                onPurchased?.Invoke(this);

                Dictionary<string, string> formFields = new Dictionary<string, string>()
                {
                    {"id", info.id }
                };

                Request.Post(Application.config.gameApiUrl + "purchases/set", formFields, UserManager.instance.data.token, (Request.Response response) =>
                {
                    callback?.Invoke(response.success);
                });
            }
        }

        public Purchase[] purchases { get { return _purchases; } private set { _purchases = value; } }
        private Purchase[] _purchases;

        private PurchasingController purchasingController;

        public void Setup(Purchase.Info[] purchases, Action<bool> callback)
        {
            Request.Get(Application.config.gameApiUrl + "purchases/get", UserManager.instance.data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    Response data = JsonUtility.FromJson<Response>(response.data);

                    this.purchases = new Purchase[purchases.Length];

                    for(int i = 0; i < this.purchases.Length; i++)
                    {
                        bool purchased = false;

                        foreach(Response.Purchase purchase in data.purchases)
                        {
                            if(purchase.id == purchases[i].id)
                            {
                                purchased = purchase.value;

                                break;
                            }
                        }

                        this.purchases[i] = new Purchase(purchases[i], purchased);
                    }

                    if(this.purchases.Length > 0)
                    {
                        purchasingController = new MobilePurchasing();

                        purchasingController.Setup(this.purchases, callback);
                    }
                    else
                    {
                        callback?.Invoke(true);
                    }
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public abstract class PurchasingController
        {
            public class SetupData
            {
                public readonly bool success;
                public readonly Purchase[] purchases;

                public class Purchase
                {
                    public readonly string id;
                    public readonly bool purchased;

                    public Purchase(string id, bool purchased)
                    {
                        this.id = id;
                        this.purchased = purchased;
                    }
                }

                public SetupData(bool success, Purchase[] purchases = null)
                {
                    this.success = success; 
                    this.purchases = purchases;
                }
            }

            public abstract void Setup(Purchase[] purchases, Action<SetupData> callback);
            public abstract void Purchase(string id, Action<bool> callback);
        }

        public class MobilePurchasing : PurchasingController, IDetailedStoreListener
        {
            private IStoreController controller;

            private Product[] products;

            private Action<SetupData> setupCallback;
            private Action<bool> purchaseCallback;

            public override void Setup(Purchase[] purchases, Action<SetupData> callback)
            {
                setupCallback = callback;

                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                foreach(Purchase purchase in purchases)
                {
                    builder.AddProduct(purchase.info.id, purchase.info.type== PurchaseManager.Purchase.Info.Type.);
                }

                UnityPurchasing.Initialize(this, builder);
            }

            public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
            {
                this.controller = controller;

                products = controller.products.all;

                List<SetupData.Purchase> purchases = new 

                setupCallback?.Invoke(new SetupData(true, ));
                setupCallback = null;
            }

            public void OnInitializeFailed(InitializationFailureReason error)
            {
                setupCallback?.Invoke(new SetupData(false));
                setupCallback = null;
            }

            public void OnInitializeFailed(InitializationFailureReason error, string message)
            {
                setupCallback?.Invoke(new SetupData(false));
                setupCallback = null;
            }

            public override void Purchase(string id, Action<bool> callback)
            {
                purchaseCallback = callback;

                controller.InitiatePurchase(id);
            }

            public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
            {
                purchaseCallback?.Invoke(true);
                purchaseCallback = null;

                return PurchaseProcessingResult.Complete;
            }

            public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
            {
                purchaseCallback?.Invoke(false);
                purchaseCallback = null;
            }

            public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
            {
                purchaseCallback?.Invoke(false);
                purchaseCallback = null;
            }
        }
    }
}
