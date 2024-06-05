using Devenant.Menu;

namespace Devenant
{
    public abstract class LoadingMenu : Menu<LoadingMenu>
    {
        public override void Open(Action callback = null)
        {
            base.Open(() =>
            {
                OnOpenLoading();

                callback?.Invoke();
            });
        }

        public override void Close(Action callback = null)
        {
            base.Close(() =>
            {
                OnCloseLoading();

                callback?.Invoke();
            });
        }

        private void Update()
        {
            if(isOpen)
            {
                OnLoading();
            }
        }

        protected abstract void OnOpenLoading();

        protected abstract void OnLoading();

        protected abstract void OnCloseLoading();
    }
}
