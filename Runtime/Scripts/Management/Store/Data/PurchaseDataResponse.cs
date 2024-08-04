namespace Devenant
{
    [System.Serializable]
    public class PurchaseDataResponse
    {
        public Purchase[] purchases;

        [System.Serializable]
        public class Purchase
        {
            public string name;
        }
    }
}
