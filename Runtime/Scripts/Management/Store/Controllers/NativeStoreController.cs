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
                    case ProductType.Consumable:

                        builder.AddProduct(product.name, UnityEngine.Purchasing.ProductType.Consumable);

                        break;

                    case ProductType.Unlockable:

                        builder.AddProduct(product.name, UnityEngine.Purchasing.ProductType.NonConsumable);

                        break;

                    case ProductType.Subscription:

                        builder.AddProduct(product.name, UnityEngine.Purchasing.ProductType.Subscription);

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

            setupProducts = null;
            setupCallback = null;

            Debug.Log("NativeStoreController: OnInitialized");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            setupCallback?.Invoke(new StoreControllerProduct[0]);

            setupProducts = null;
            setupCallback = null;

            Debug.LogError("NativeStoreController: OnInitializeFailed => " + error.ToString());
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            setupCallback?.Invoke(new StoreControllerProduct[0]);

            setupProducts = null;
            setupCallback = null;

            Debug.LogError("NativeStoreController: OnInitializeFailed => " + error.ToString() + ": " + message.ToString());
        }

        public override void Purchase(StoreControllerProduct product, Action<PurchaseResponse> callback)
        {
            purchaseProduct = product;
            purchaseCallback = callback;

            controller.InitiatePurchase(product.product.name);

            Debug.Log("NativeStoreController: Purchase");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(true, purchaseProduct, purchaseEvent.purchasedProduct.transactionID, purchaseEvent.purchasedProduct.receipt));

            purchaseProduct = null;
            purchaseCallback = null;

            Debug.Log("NativeStoreController: ProcessPurchase");

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureDescription failureDescription)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(false, purchaseProduct, product.transactionID, product.receipt));

            purchaseProduct = null;
            purchaseCallback = null;

            Debug.LogError("NativeStoreController: OnPurchaseFailed => " + failureDescription.reason.ToString() + ": " + failureDescription.message.ToString());
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            purchaseCallback?.Invoke(new PurchaseResponse(false, purchaseProduct, product.transactionID, product.receipt));

            purchaseProduct = null;
            purchaseCallback = null;

            purchaseCallback = null;

            Debug.LogError("NativeStoreController: OnPurchaseFailed => " + failureReason.ToString());
        }
    }
}
