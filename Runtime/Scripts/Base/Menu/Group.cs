namespace Devenant.Menu
{
    public class Group
    {
        public readonly IMenu[] tabs;

        public int tab { get { return _tab; } private set { _tab = value; } }
        private int _tab;

        public Group(IMenu[] tabs, Action callback = null)
        {
            this.tabs = tabs;

            tabs[0].Open(callback);
        }

        public void Open(int index, Action callback = null)
        {
            if(tab == index)
            {
                return;
            }

            if(index < 0 || index >= tabs.Length)
            {
                return;
            }

            if (tab != -1)
            {
                tabs[tab].Close();
            }

            tabs[index].Open(callback);

            tab = index;
        }

        public void Close()
        {
            tabs[tab].Close();

            tab = -1;
        }
    }
}
