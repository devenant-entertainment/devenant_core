using UnityEngine;

namespace Devenant
{
    public enum PurchaseType
    {
        Purchase
    }

    public class Purchase : Icon
    {
        public readonly PurchaseType type;

        public string price;
        public bool purchased;

        public Purchase(string name, Sprite icon, PurchaseType type) : base (name, icon)
        {
            this.type = type;
        }

        public Purchase(SOPurchase purchase) : base(purchase) 
        {
            type = purchase.type;
        }
    }
}
