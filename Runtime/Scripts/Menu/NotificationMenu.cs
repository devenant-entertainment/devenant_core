using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
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

    public class NotificationMenu : Menu<NotificationMenu>
    {
        [SerializeField] private float openTime = 3;
        [SerializeField] private RectTransform panelTransform;
        [SerializeField] private Button actionButton;
        [SerializeField] private TextMeshProUGUI messageText;

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
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(() => 
            { 
                notifications[0].action?.Invoke(); 
            });

            messageText.text = LocalizationManager.instance.Translate("Notification", notifications[0].message);

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);

            base.Open(() =>
            {
                StartCoroutine(CloseCoroutine());
            });

            IEnumerator CloseCoroutine()
            {
                yield return new WaitForSeconds(openTime);

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