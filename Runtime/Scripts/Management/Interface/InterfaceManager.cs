using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class InterfaceManager : Singleton<InterfaceManager>
    {
        public void SetScale(float scale)
        {
            foreach(CanvasScaler canvasScaler in FindObjectsByType<CanvasScaler>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                canvasScaler.referenceResolution = new Vector2(1000, 1000 * scale);
            }
        }
    }
}
