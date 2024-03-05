using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class InterfaceManager : Singleton<InterfaceManager>
    {
        public void SetScale(float scale)
        {
            foreach(CanvasScaler canvasScaler in FindObjectsOfType<CanvasScaler>())
            {
                canvasScaler.referenceResolution = new Vector2(1000, 1000 * scale);
            }
        }
    }
}
