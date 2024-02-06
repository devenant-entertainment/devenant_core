using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>, IDetailedStoreListener
    {
        [System.Serializable]
        private class PurchaseList
        {
            [System.Serializable]
            public class Purchase
            {
                public string id;
                public string type;
                public bool value;
            }

            public Purchase[] purchases;
        }

        private IStoreController controller;
        private IExtensionProvider extensions;

        public Product[] products { get { return _products; } private set { _products = value; } }
        private Product[] _products;

        public List<string> purchases { get { return _purchases; } private set { _purchases = value; } }
        private List<string> _purchases;

        private Action<bool> setupCallback;
        private Action<bool> purchaseCallback;

        public void Setup(Action<bool> callback)
        {
            setupCallback = callback;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            Request.Get(Application.config.apiUrl + "purchases", UserManager.instance.data.token, (Request.Response response) =>
            {
                if(response.success)
                {
                    PurchaseList purchaseList = JsonUtility.FromJson<PurchaseList>(response.data);

                    foreach(PurchaseList.Purchase purchase in purchaseList.purchases)
                    {
                        builder.AddProduct(purchase.id, System.Enum.Parse<ProductType>(purchase.type));
                    }

                    if(builder.products.Count > 0)
                    {
                        UnityPurchasing.Initialize(this, builder);
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

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;
            this.extensions = extensions;

            products = controller.products.all;

            setupCallback?.Invoke(true);
            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            setupCallback?.Invoke(false);
            setupCallback = null;

            Debug.LogError("OnInitializeFailed: " + error.ToString());
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            setupCallback?.Invoke(false);
            setupCallback = null;

            Debug.LogError("OnInitializeFailed: " + error.ToString() + " => message: " + message);
        }

        public void Purchase(Product product, Action<bool> callback)
        {
            purchaseCallback = callback;

            controller.InitiatePurchase(product.definition.id);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            purchaseCallback?.Invoke(false);
            purchaseCallback = null;

            Debug.LogError("OnPurchaseFailed: " + failureDescription.message.ToString());
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            purchaseCallback?.Invoke(false);
            purchaseCallback = null;

            Debug.LogError("OnPurchaseFailed: " + failureReason.ToString());
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            CreatePurchase(purchaseEvent.purchasedProduct.definition.id, (bool success) =>
            {
                purchaseCallback?.Invoke(success);
                purchaseCallback = null;
            });

            Debug.LogError("ProcessPurchase...");

            return PurchaseProcessingResult.Complete;
        }

        private void CreatePurchase(string product, Action<bool> callback = null)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token },
                { "product", product }
            };

            Request.Post(Application.config.apiUrl + "purchase/create", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    GetPurchases((bool success) =>
                    {
                        callback?.Invoke(success);
                    });
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void GetPurchases(Action<bool> callback)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "token", UserManager.instance.data.token }
            };

            Request.Post(Application.config.apiUrl + "purchase/get", formFields, (Request.Response response) =>
            {
                if(response.success)
                {
                    purchases = new List<string>();

                    foreach(PurchaseList.Purchase purchase in JsonUtility.FromJson<PurchaseList>(response.data).purchases)
                    {
                        purchases.Add(purchase.id);
                    }

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(true);
                }
            });
        }
    }
}
