using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class NotificationMenu : Menu<NotificationMenu>
    {
        [SerializeField] private RectTransform panelTransform;
        [SerializeField] private TextMeshProUGUI messageText;

        private List<string> messages = new List<string>();

        public void Open(string message)
        {
            if(!messages.Contains(message))
            {
                messages.Add(message);
            }

            if(!isOpen)
            {
                Open();
            }
        }

        private void Open()
        {
            messageText.text = LocalizationManager.instance.Translate("message", messages[0]);

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);

            base.Open(() =>
            {
                StartCoroutine(CloseCoroutine());
            });

            IEnumerator CloseCoroutine()
            {
                yield return new WaitForSeconds(3);

                Close(() =>
                {
                    messages.RemoveAt(0);

                    if(messages.Count > 0)
                    {
                        Open();
                    }
                });
            }
        }
    }
}