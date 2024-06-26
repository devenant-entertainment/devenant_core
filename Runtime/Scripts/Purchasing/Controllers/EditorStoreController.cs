using System.Collections.Generic;

namespace Devenant
{
    public class EditorStoreController : StoreController
    {
        public override void Setup(Product[] products, Action<StoreProduct[]> callback)
        {
            List<StoreProduct> storeProducts = new List<StoreProduct>();

            foreach(Product product in products)
            {
                storeProducts.Add(new StoreProduct(product, "0.00$"));
            }

            callback?.Invoke(storeProducts.ToArray());
        }

        public override void Purchase(StoreProduct product, Action<Purchase> callback)
        {
            callback?.Invoke(new Purchase(true, product, "editorTransactionData", "editorReceiptData"));
        }
    }
}
