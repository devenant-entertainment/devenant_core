namespace Devenant
{
    public interface IMenu
    {
        public void Open(Action callback = null);
        public void Close(Action callback = null);
    }
}
