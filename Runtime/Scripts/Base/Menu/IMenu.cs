namespace Devenant.Menu
{
    public interface IMenu
    {
        public void Open(Action callback = null);
        public void Close(Action callback = null);
    }
}
