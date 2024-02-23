using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing;

namespace Devenant
{
    public class MobileStoreController : StoreController, IDetailedStoreListener
    {
        private IStoreController controller;

        private Product[] products;

        private Action<bool> setupCallback;
        private Action<StorePurchaseResponse> purchaseCallback;

        public override void Setup(PurchaseData[] purchases, Action<bool> callback)
        {
            setupCallback = callback;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach(PurchaseData purchaseInfo in purchases)
            {
                builder.AddProduct(purchaseInfo.name, ProductType.NonConsumable);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;

            products = controller.products.all;

            setupCallback?.Invoke(true);

            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            setupCallback?.Invoke(false);

            setupCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            setupCallback?.Invoke(false);

            setupCallback = null;
        }

        public override void Purchase(string id, Action<StorePurchaseResponse> callback)
        {
            purchaseCallback = callback;

            controller.InitiatePurchase(id);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            purchaseCallback?.Invoke(new StorePurchaseResponse(true, purchaseEvent.purchasedProduct.transactionID));

            purchaseCallback = null;

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            purchaseCallback?.Invoke(new StorePurchaseResponse(false, string.Empty));

            purchaseCallback = null;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            purchaseCallback?.Invoke(new StorePurchaseResponse(false, string.Empty));

            purchaseCallback = null;
        }

        public override StorePurchase[] GetStorePurchases()
        {
            List<StorePurchase> purchases = new List<StorePurchase>();

            foreach(Product product in products)
            {
                purchases.Add(new StorePurchase(product.definition.id, product.metadata.localizedPriceString, product.hasReceipt));
            }

            return purchases.ToArray();
        }
    }
}
