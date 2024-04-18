using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class MenuGroup
    {
        public readonly IMenu[] tabs;

        public MenuGroup(IMenu[] tabs, Action callback = null)
        {
            this.tabs = tabs;

            Open(0, callback);
        }

        public void Open(int index, Action callback = null)
        {
            for(int i = 0; i < tabs.Length; i++)
            {
                if(i == index)
                {
                    tabs[i].Open(callback);
                }
                else
                {
                    tabs[i].Close();
                }
            }
        }
    }
}
