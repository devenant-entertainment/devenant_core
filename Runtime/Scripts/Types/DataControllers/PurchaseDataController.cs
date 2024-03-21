using System.Collections.Generic;

namespace Devenant
{
    public class PurchaseDataController : AssetDataController<Purchase, SOPurchase>
    {
        protected override List<Purchase> NormalizeData(SOPurchase[] data)
        {
            List<Purchase> result = new List<Purchase>();

            foreach(SOPurchase purchase in data)
            {
                result.Add(new Purchase(purchase.name, purchase.icon, purchase.type));
            }

            return result;
        }
    }
}
