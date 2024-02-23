using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private Rect lastSafeArea;

        private void Start()
        {
            Apply();
        }

        private void OnRectTransformDimensionsChange()
        {
            if(lastSafeArea != Screen.safeArea)
            {
                Apply();
            }
        }

        private void Apply()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

            lastSafeArea = Screen.safeArea;
        }
    }
}