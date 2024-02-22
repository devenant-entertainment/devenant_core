using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    [System.Serializable]
    public class PurchaseResponse
    {
        public Purchase[] purchases;

        [System.Serializable]
        public class Purchase
        {
            public string id;
            public bool value;
        }
    }
}
