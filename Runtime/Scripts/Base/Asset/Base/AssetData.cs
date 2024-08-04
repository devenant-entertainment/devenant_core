using UnityEngine;

namespace Devenant
{
    public class AssetData
    {
        public readonly string name;
        public readonly Sprite icon;

        public AssetData (AssetDataAsset asset)
        {
            name = asset.name;
            icon = asset.icon;
        }
    }
}
