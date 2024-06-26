using UnityEngine;

namespace Devenant
{
    public class Asset
    {
        public readonly string name;
        public readonly Sprite icon;

        public Asset(string name, Sprite icon)
        {
            this.name = name;
            this.icon = icon;
        }

        public Asset (SOAsset asset)
        {
            name = asset.name;
            icon = asset.icon;
        }
    }
}
