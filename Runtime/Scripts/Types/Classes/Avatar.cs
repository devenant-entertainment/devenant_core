using UnityEngine;

namespace Devenant
{
    public class Avatar : Unlockable
    {
        public Avatar(string name, Sprite icon, Achievement achievement, Purchase purchase) : base(name, icon, achievement, purchase)
        {

        }

        public Avatar (SOAvatar avatar) : base(avatar)
        {

        }
    }
}
