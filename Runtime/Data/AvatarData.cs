using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "Avatar", menuName = "Devenant/Avatar")]
    public class AvatarData : ScriptableObject
    {
        public PurchaseData purchase;
        public Sprite sprite;
    }
}
