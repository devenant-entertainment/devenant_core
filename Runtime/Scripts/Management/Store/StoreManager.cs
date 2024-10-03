using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class StoreManager : Singleton<StoreManager>, IInitializable
    {
        public static Action<StoreControllerProduct> onPurchased;

        public EntityDataArray<ProductData> products;

        public StoreControllerProduct[] storeProducts { get { return _storeProducts; } private set { _storeProducts = value; } }
        private StoreControllerProduct[] _storeProducts;

        private StoreController storeController;

        public void Initialize(Action<InitializationResponse> callback)
        {
            Addressables.LoadAssetsAsync<ProductAsset>(typeof(ProductAsset).Name, null).Completed += (AsyncOperationHandle<IList<ProductAsset>> asyncOperationHandle) =>
            {
                List<ProductData> products = new List<ProductData>();

                foreach (ProductAsset soProduct in asyncOperationHandle.Result)
                {
                    products.Add(new ProductData(soProduct));
                }

                this.products = new EntityDataArray<ProductData>(products.ToArray());

                if (this.products.Get().Length > 0)
                {
                    switch (Application.platform)
                    {
                        default:

                            storeController = new EditorStoreController();

                            break;

                        case RuntimePlatform.Android | RuntimePlatform.IPhonePlayer:

                            storeController = new NativeStoreController();

                            break;
                    }

                    storeController.Setup(this.products.Get(), (StoreControllerProduct[] products) =>
                    {
                        if(products.Length > 0)
                        {
                            this.storeProducts = products;

                            Dictionary<string, string> formFields = new Dictionary<string, string>
                            {
                                { "token", UserManager.instance.data.token }
                            };

                            Request.Post(BackendManager.instance.data.purchaseGet, formFields, (Request.Response response) =>
                            {
                                if(response.success)
                                {
                                    PurchaseDataResponse responseData = JsonUtility.FromJson<PurchaseDataResponse>(response.data);

                                    foreach(PurchaseDataResponse.Purchase purchase in responseData.purchases)
                                    {
                                        foreach(StoreControllerProduct product in this.storeProducts)
                                        {
                                            if(purchase.name == product.product.name)
                                            {
                                                product.value = true;
                                            }
                                        }
                                    }

                                    callback?.Invoke(new InitializationResponse(true));
                                }
                                else
                                {
                                    Debug.LogError("StoreManager Error: There was an error during user purchase get call");

                                    callback?.Invoke(new InitializationResponse(false));
                                }
                            });
                        }
                        else
                        {
                            Debug.LogError("StoreManager Error: Store product list is empty");

                            callback?.Invoke(new InitializationResponse(false));
                        }
                    });
                }
                else
                {
                    Debug.LogError("StoreManager Error: Local product list is empty");

                    callback?.Invoke(new InitializationResponse(false));
                }
            };
        }

        public void Purchase(StoreControllerProduct product, Action<bool> callback = null)
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

            if(product.product.type != ProductType.Consumable && product.value == true)
            {
                callback?.Invoke(false);

                return;
            }

            storeController.Purchase(product, (PurchaseResponse purchase) =>
            {
                if(purchase.success)
                {
                    product.value = true;

                    Dictionary<string, string> purchaseFormFields = new Dictionary<string, string>()
                    {
                        { "token", UserManager.instance.data.token },
                        { "name", product.product.name },
                        { "type", product.product.type.ToString() },
                        { "identifier", Application.identifier },
                        { "platform", Application.platform.ToString() },
                        { "transaction", purchase.transaction }
                    };

                    Request.Post(BackendManager.instance.data.purchaseSet, purchaseFormFields, (Request.Response response) =>
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
    }
}
