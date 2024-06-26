namespace Devenant
{
    [System.Serializable]
    public class ProductResponse
    {
        public Purchase[] purchases;

        [System.Serializable]
        public class Purchase
        {
            public string name;
        }
    }
}
