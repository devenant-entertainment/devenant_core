using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "purchase_", menuName = "Devenant/Core/Purchase")]
    public class SOPurchase : SOIcon
    {
        public PurchaseType type;
    }
}
