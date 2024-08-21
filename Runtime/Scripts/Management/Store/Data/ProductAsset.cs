using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "product_", menuName = "Devenant/Core/Purchase")]
    public class ProductAsset : EntityAsset
    {
        public ProductType type;
    }
}
