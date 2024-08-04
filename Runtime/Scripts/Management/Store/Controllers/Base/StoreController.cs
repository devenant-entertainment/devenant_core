using System;

namespace Devenant
{
    public abstract class StoreController
    {
        public abstract void Setup(ProductData[] products, Action<StoreControllerProduct[]> callback);
        public abstract void Purchase(StoreControllerProduct product, Action<PurchaseResponse> callback);
    }
}