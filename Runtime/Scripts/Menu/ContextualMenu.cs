using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class ContextualMenu : Menu<ContextualMenu>
    {
        public struct Option
        {
            public string name;
            public Action action;
        }

        [SerializeField] private RectTransform optionHolder;
        [SerializeField] private GameObject optionElement;

        [SerializeField] private Button closeButton;

        private Content optionContent;

        public void Open(Option[] options)
        {
            optionContent?.Clear();

            optionContent = new Content(optionHolder, optionElement);

            foreach(Option option in options)
            {
                GameObject newOption = optionContent.Create();

                newOption.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.Translate("interface", option.name);

                newOption.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Close(() =>
                    {
                        option.action?.Invoke();
                    });
                });
            }

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() => 
            {
                Close();
            });

            LayoutRebuilder.ForceRebuildLayoutImmediate(optionHolder);

            Vector2 pivot = Vector2.zero;
            pivot.x = Input.mousePosition.x > Screen.width / 2 ? 1 : 0;
            pivot.y = Input.mousePosition.y > Screen.height / 2 ? 1 : 0;
            optionHolder.pivot = pivot;

            optionHolder.transform.position = Input.mousePosition;

            base.Open();
        }
    }
}