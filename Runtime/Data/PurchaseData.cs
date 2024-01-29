using UnityEngine;
using UnityEngine.Purchasing;

namespace Devenant
{
    [CreateAssetMenu(fileName = "NewPurchase", menuName = "Devenant/Purchase")]
    public class PurchaseData : ScriptableObject
    {
        public ProductType type;
        public PlatformValue<string> id;

        public bool purchased { get { return _purchased; } set { _purchased = value; } }
        private bool _purchased = false;
    }
}
