using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using System.Linq;
using UnityEngine;

namespace Devenant
{
    public class NativeStoreController : StoreController, IDetailedStoreListener
    {
        private IStoreController controller;

        private ProductData[] setupProducts;
        private Action<StoreControllerProduct[]> setupCallback;

        private StoreControllerProduct purchaseProduct;
        private Action<PurchaseResponse> purchaseCallback;

        public override void Setup(ProductData[] products, Action<StoreControllerProduct[]> callback)
        {
            setupProducts = products;
            setupCallback = callback;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach(ProductData product in products)
            {
                switch(product.type)
                {
                    case ProductType.Subscription:

                        builder.AddProduct(product.name, UnityEngine.Purchasing.ProductType.Subscription);

                        break;

                    default:

                        builder.AddProduct(product.name, UnityEngine.Purchasing.ProductType.Consumable);

                        break;
                }
            }

            UnityPurchasing.Initialize(this, builder);

            Debug.Log("NativeStoreController: Setup");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;

            List<UnityEngine.Purchasing.Product> nativeProducts = controller.products.all.ToList();

            List<StoreControllerProduct> storeProducts = new List<StoreControllerProduct>();

            foreach(ProductData product in setupProducts)
            {
                UnityEngine.Purchasing.Product nativeProduct = nativeProducts.Find((x) => x.definition.id == product.name);

                storeProducts.Add(new StoreControllerProduct(product, nativeProduct.metadata.localizedPriceString));
            }

            setupCallback?.Invoke(storeProducts.ToArray());

            Debug.Log("NativeStoreController: OnInitialized");

            setupProducts = null;
            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            setupCallback?.Invoke(new StoreControllerProduct[0]);

            Debug.LogError("NativeStoreController: OnInitializeFailed => " + error.ToString());

            setupProducts = null;
            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            setupCallback?.Invoke(new StoreControllerProduct[0]);

            Debug.LogError("NativeStoreController: OnInitializeFailed => " + error.ToString() + ": " + message.ToString());

            setupProducts = null;
            setupCallback = null;
        }

        public override void Purchase(StoreControllerProduct product, Action<PurchaseResponse> callback)
        {
            purchaseProduct = product;
            purchaseCallback = callback;

            controller.InitiatePurchase(product.product.name);

            Debug.Log("NativeStoreController: Purchassing " + product.product.name);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(true, purchaseProduct, purchaseEvent.purchasedProduct.transactionID));

            Debug.Log("NativeStoreController: ProcessPurchase => " + purchaseProduct.product.name + " / " + purchaseEvent.purchasedProduct.transactionID);

            purchaseProduct = null;
            purchaseCallback = null;

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureDescription failureDescription)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(false, purchaseProduct, product.transactionID));

            Debug.LogError("NativeStoreController: OnPurchaseFailed => " + failureDescription.reason.ToString() + ": " + failureDescription.message.ToString());

            purchaseProduct = null;
            purchaseCallback = null;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(false, purchaseProduct, product.transactionID));

            Debug.LogError("NativeStoreController: OnPurchaseFailed => " + failureReason.ToString());

            purchaseProduct = null;
            purchaseCallback = null;

            purchaseCallback = null;
        }
    }
}
