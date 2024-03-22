using UnityEngine;

namespace Devenant
{
    public class Icon : Asset
    {
        public readonly Sprite icon;

        public Icon(string name, Sprite icon) : base(name)
        {
            this.icon = icon;
        }

        public Icon (SOIcon icon) : base(icon)
        {
            this.icon = icon.icon;
        }
    }
}
