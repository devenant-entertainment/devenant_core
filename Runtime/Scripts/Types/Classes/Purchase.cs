namespace Devenant
{
    public enum PurchaseType
    {
        Purchase
    }

    public class Purchase
    {
        public readonly string name;
        public readonly PurchaseType type;

        public string price;
        public bool purchased;

        public Purchase(string name, PurchaseType type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
