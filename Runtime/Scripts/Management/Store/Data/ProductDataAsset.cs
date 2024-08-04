using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "purchase_", menuName = "Devenant/Core/Purchase")]
    public class ProductDataAsset : AssetDataAsset
    {
        public ProductType type;
    }
}
