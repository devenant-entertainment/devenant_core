using UnityEngine;

namespace Devenant
{
    public class Avatar
    {
        public readonly string id;
        public readonly Purchase purchase;

        public Avatar (string id, Purchase purchase = null)
        {
            this.id = id;
            this.purchase = purchase;
        }
    }
}
