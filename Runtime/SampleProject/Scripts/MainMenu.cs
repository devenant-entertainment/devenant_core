using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant.Samples
{
    public class MainMenu : Menu<MainMenu>
    {
        public override void Open(Action callback = null)
        {
            Debug.Log("Done!");

            base.Open(callback);
        }
    }
}