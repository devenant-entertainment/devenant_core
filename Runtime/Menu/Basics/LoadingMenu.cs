using UnityEngine;

namespace Devenant
{
    public class LoadingMenu : Menu<LoadingMenu>
    {
        [SerializeField] private RectTransform inIcon;
        [SerializeField] private RectTransform outIcon;

        private void Update()
        {
            if(isOpen)
            {
                inIcon.Rotate(0, 0, -180 * Time.deltaTime);
                outIcon.Rotate(0, 0, 180 * Time.deltaTime);
            }
        }
    }
}
