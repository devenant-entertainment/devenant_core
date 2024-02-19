using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class NotificationMenu : Menu<NotificationMenu>
    {
        public class Notification
        {
            public readonly string message;
            public readonly Action action;

            public Notification(string message)
            {
                this.message = message;
            }

            public Notification(string message, Action action)
            {
                this.message = message;
                this.action = action;
            }
        }

        [SerializeField] private RectTransform panelTransform;

        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button actionButton;

        private List<Notification> notifications = new List<Notification>();

        public void Open(Notification notification)
        {
            notifications.Add(notification);

            if(!isOpen)
            {
                Show();
            }
        }

        private void Show()
        {
            messageText.text = LocalizationManager.instance.Translate("message", notifications[0].message);

            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => 
            { 
                notifications[0].action?.Invoke(); 
            });

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
                    notifications.RemoveAt(0);

                    if(notifications.Count > 0)
                    {
                        Show();
                    }
                });
            }
        }
    }
}