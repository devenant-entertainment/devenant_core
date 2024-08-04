using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "product_", menuName = "Devenant/Core/Purchase")]
    public class ProductDataAsset : AssetDataAsset
    {
        public ProductType type;
    }
}
