using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;
using System.Linq;

namespace Devenant
{
    public class NativeStoreController : StoreController, IDetailedStoreListener
    {
        private IStoreController controller;

        private Product[] setupProducts;
        private Action<StoreProduct[]> setupCallback;

        private StoreProduct purchaseProduct;
        private Action<Purchase> purchaseCallback;

        public override void Setup(Product[] products, Action<StoreProduct[]> callback)
        {
            setupProducts = products;
            setupCallback = callback;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach(Product product in products)
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
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;

            List<UnityEngine.Purchasing.Product> nativeProducts = controller.products.all.ToList();

            List<StoreProduct> storeProducts = new List<StoreProduct>();

            foreach(Product product in setupProducts)
            {
                UnityEngine.Purchasing.Product nativeProduct = nativeProducts.Find((x) => x.definition.id == product.name);

                storeProducts.Add(new StoreProduct(product, nativeProduct.metadata.localizedPriceString));
            }

            setupCallback?.Invoke(storeProducts.ToArray());

            setupProducts = null;
            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            setupCallback?.Invoke(new StoreProduct[0]);

            setupProducts = null;
            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            setupCallback?.Invoke(new StoreProduct[0]);

            setupProducts = null;
            setupCallback = null;
        }

        public override void Purchase(StoreProduct product, Action<Purchase> callback)
        {
            purchaseProduct = product;
            purchaseCallback = callback;

            controller.InitiatePurchase(product.product.name);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            purchaseCallback?.Invoke(new Purchase(true, purchaseProduct, purchaseEvent.purchasedProduct.transactionID, purchaseEvent.purchasedProduct.receipt));

            purchaseProduct = null;
            purchaseCallback = null;

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureDescription failureDescription)
        {
            purchaseCallback?.Invoke(new Purchase(false, purchaseProduct, product.transactionID, product.receipt));

            purchaseProduct = null;
            purchaseCallback = null;
        }

        public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
        {
            purchaseCallback?.Invoke(new Purchase(false, purchaseProduct, product.transactionID, product.receipt));

            purchaseProduct = null;
            purchaseCallback = null;

            purchaseCallback = null;
        }
    }
}
