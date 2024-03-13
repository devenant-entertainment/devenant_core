using System.Collections.Generic;
using System.Threading.Tasks;

namespace Devenant
{
    public abstract class DataContent<T, D> where D : SOAsset
    {
        protected List<T> values;

        public void Setup(Action<T[]> callback)
        {
            AssetManager.instance.Get((D[] result) =>
            {
                values = SetupData(result);

                callback?.Invoke(Get());
            });
        }

        public async Task<T[]> SetupAsync()
        {
            TaskCompletionSource<T[]> taskCompletionSource = new TaskCompletionSource<T[]>();

            AssetManager.instance.Get((D[] result) =>
            {
                values = SetupData(result);

                taskCompletionSource.SetResult(Get());
            });

            return await taskCompletionSource.Task;
        }

        protected abstract List<T> SetupData(D[] data);

        public T Get(string name)
        {
            return Find(name);
        }

        protected abstract T Find(string name);

        public T[] Get()
        {
            return values.ToArray();
        }

        protected Achievement GetAchievement(SOAchievement achievement)
        {
            return achievement != null ? AchievementManager.instance.achievements.Get(achievement.name) : null;
        }

        protected Purchase GetPurchase(SOPurchase purchase)
        {
            return purchase != null ? PurchaseManager.instance.purchases.Get(purchase.name) : null;
        }
    }
}
