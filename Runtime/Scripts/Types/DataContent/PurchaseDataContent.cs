using System.Collections.Generic;

namespace Devenant
{
    public class PurchaseDataContent : DataContent<Purchase, SOPurchase>
    {
        protected override Purchase Find(string name)
        {
            return values.Find((x) => x.name == name);
        }

        protected override List<Purchase> SetupData(SOPurchase[] data)
        {
            List<Purchase> result = new List<Purchase>();

            foreach(SOPurchase purchase in data)
            {
                result.Add(new Purchase(purchase.name, purchase.type));
            }

            return result;
        }
    }
}
