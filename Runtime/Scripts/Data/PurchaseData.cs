using UnityEngine;
using UnityEngine.Purchasing;

namespace Devenant
{
    [CreateAssetMenu(fileName = "NewPurchase", menuName = "Devenant/Purchase")]
    public class PurchaseData : ScriptableObject
    {
        public ProductType type;
        public string id;
    }
}
