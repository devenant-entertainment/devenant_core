using UnityEngine;

namespace Devenant
{
    public enum ProductType
    {
        Consumable,
        Unlockable,
        Subscription
    }

    public class Product : Asset
    {
        public readonly ProductType type;

        public Product(string name, Sprite icon, ProductType type) : base (name, icon)
        {
            this.type = type;
        }

        public Product(SOProduct product) : base(product) 
        {
            type = product.type;
        }
    }
}
