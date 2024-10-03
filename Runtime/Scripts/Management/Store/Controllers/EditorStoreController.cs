using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class EditorStoreController : StoreController
    {
        public override void Setup(ProductData[] products, Action<StoreControllerProduct[]> callback)
        {
            List<StoreControllerProduct> storeProducts = new List<StoreControllerProduct>();

            foreach(ProductData product in products)
            {
                storeProducts.Add(new StoreControllerProduct(product, "0.00$"));
            }

            callback?.Invoke(storeProducts.ToArray());
        }

        public override void Purchase(StoreControllerProduct product, Action<PurchaseResponse> callback)
        {
            callback?.Invoke(new PurchaseResponse(true, product, "editorTransactionData"));

            Debug.Log("NativeStoreController: Purchassing " + product.product.name);
        }
    }
}
