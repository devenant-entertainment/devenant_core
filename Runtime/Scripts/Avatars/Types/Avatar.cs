using UnityEngine;

namespace Devenant
{
    public class Avatar : UnlockableAsset
    {
        public Avatar(string name, Sprite icon, Achievement achievement, Product product) : base(name, icon, achievement, product)
        {

        }

        public Avatar (SOAvatar avatar) : base(avatar)
        {

        }
    }
}
