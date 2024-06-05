using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "purchase_", menuName = "Devenant/Core/Purchase")]
    public class SOPurchase : SOAsset
    {
        public PurchaseType type;
    }
}
