using UnityEngine;

namespace Devenant
{
    public class EntityData
    {
        public readonly string name;
        public readonly Sprite icon;

        public EntityData (EntityAsset asset)
        {
            name = asset.name;
            icon = asset.icon;
        }
    }
}
