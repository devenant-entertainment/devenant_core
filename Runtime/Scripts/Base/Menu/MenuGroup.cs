
using UnityEditor;
using UnityEngine;

namespace Devenant
{
    public class MenuGroup
    {
        public readonly IMenu[] tabs;

        public int tab { get { return _tab; } private set { _tab = value; } }
        private int _tab;

        public MenuGroup(IMenu[] tabs, Action callback = null)
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
