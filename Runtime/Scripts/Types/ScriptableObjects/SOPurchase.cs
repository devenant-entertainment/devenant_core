using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "purchase_", menuName = "Devenant/Core/Purchase", order = 20)]
    public class SOPurchase : SOAsset
    {
        public PurchaseType type;
    }
}
