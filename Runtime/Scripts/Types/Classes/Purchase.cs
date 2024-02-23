using System.Collections.Generic;

namespace Devenant
{
    public enum PurchaseType
    {
        Purchase
    }

    public class Purchase
    {
        public Action<Purchase> onUpdated;

        public readonly string id;
        public readonly PurchaseType type;
        public readonly string price;

        public bool purchased { get { return _purchased; } private set { _purchased = value; } }
        private bool _purchased;

        public Purchase(string id, PurchaseType type, string price, bool purchased)
        {
            this.id = id;
            this.type = type;
            this.price = price;
            this.purchased = purchased;
        }

        public void Set(bool purchased, string transaction, Action<bool> callback = null)
        {
            if (this.purchased == purchased)
            {
                callback?.Invoke(false);

                return;
            }

            this.purchased = purchased;

            onUpdated?.Invoke(this);

            Dictionary<string, string> formFields = new Dictionary<string, string>()
            {
                {"id", id },
                {"purchased", purchased.ToString() },
                {"platform", UnityEngine.Application.platform.ToString() },
                {"transaction", transaction }
            };

            Request.Post(ApplicationManager.instance.backend.purchaseSet, formFields, UserManager.instance.user.token, (Request.Response response) =>
            {
                callback?.Invoke(response.success);
            });
        }
    }
}
