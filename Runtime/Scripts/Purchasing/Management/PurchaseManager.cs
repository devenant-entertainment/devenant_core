using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class PurchaseManager : Singleton<PurchaseManager>
    {
        public static Action<StoreProduct> onPurchased;

        public AssetArray<Product> products;

        public StoreProduct[] storeProducts { get { return _storeProducts; } private set { _storeProducts = value; } }
        private StoreProduct[] _storeProducts;

        private StoreController storeController;

        public void Setup(Action<bool> callback)
        {
            AssetManager.instance.GetAll((SOProduct[] soProducts) =>
            {
                if(soProducts.Length > 0)
                {
                    SetupProducts(soProducts);

                    SetupController();

                    storeController.Setup(products.Get(), (StoreProduct[] products) =>
                    {
                        if(products.Length > 0)
                        {
                            this.storeProducts = products;

                            Dictionary<string, string> formFields = new Dictionary<string, string>
                            {
                                { "token", UserManager.instance.user.token }
                            };

                            Request.Post(ApplicationManager.instance.backend.purchaseGet, formFields, (Request.Response response) =>
                            {
                                if(response.success)
                                {
                                    PurchaseResponse responseData = JsonUtility.FromJson<PurchaseResponse>(response.data);

                                    foreach(PurchaseResponse.Purchase purchase in responseData.purchases)
                                    {
                                        foreach(StoreProduct product in this.storeProducts)
                                        {
                                            if(purchase.name == product.product.name)
                                            {
                                                product.value = true;
                                            }
                                        }
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
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        public void Purchase(StoreProduct product, Action<bool> callback = null)
        {
            if(storeController == null)
            {
                callback?.Invoke(false);

                return;
            }

            if(product == null)
            {
                callback?.Invoke(false);

                return;
            }

            if(product.value == true)
            {
                callback?.Invoke(false);

                return;
            }

            storeController.Purchase(product, (Purchase purchase) =>
            {
                if(purchase.success)
                {
                    product.value = true;

                    Dictionary<string, string> purchaseFormFields = new Dictionary<string, string>()
                    {
                        { "token", UserManager.instance.user.token },
                        { "name", product.product.name },
                        { "type", product.product.type.ToString() },
                        { "identifier", UnityEngine.Application.identifier },
                        { "platform", ApplicationManager.instance.application.platform.ToString() },
                        { "transaction", purchase.transaction },
                        { "receipt", purchase.receipt }
                    };

                    Request.Post(ApplicationManager.instance.backend.purchaseSet, purchaseFormFields, (Request.Response response) =>
                    {
                        callback?.Invoke(response.success);

                        onPurchased?.Invoke(product);
                    });
                }
                else
                {
                    callback?.Invoke(false);
                }
            });
        }

        private void SetupProducts(SOProduct[] soProducts)
        {
            List<Product> products = new List<Product>();

            foreach(SOProduct soProduct in soProducts)
            {
                products.Add(new Product(soProduct));
            }

            this.products = new AssetArray<Product>(products.ToArray());
        }

        private void SetupController()
        {
            switch(ApplicationManager.instance.application.platform)
            {
                case ApplicationPlatform.Editor:

                    storeController = new EditorStoreController();

                    break;

                default:

                    switch(ApplicationManager.instance.application.environment)
                    {
                        case ApplicationEnvironment.Development:

                            storeController = new EditorStoreController();

                            break;

                        case ApplicationEnvironment.Production:

                            storeController = new NativeStoreController();

                            break;
                    }

                    break;
            }
        }
    }
}
