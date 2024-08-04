using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class MessageMenu : Menu<MessageMenu>
    {
        [SerializeField] private RectTransform panelTransform;
        [SerializeField] private TextMeshProUGUI messageText;

        [Header("Single")]
        [SerializeField] private GameObject singleSection;
        [SerializeField] private Button singleAcceptButton;

        [Header("Multiple")]
        [SerializeField] private GameObject multipleSection;
        [SerializeField] private Button multipleAcceptButton;
        [SerializeField] private Button multipleCancelButton;

        public void Open(string message, Action callback = null)
        {
            singleSection.SetActive(true);
            multipleSection.SetActive(false);

            singleAcceptButton.onClick.RemoveAllListeners();
            singleAcceptButton.onClick.AddListener(() =>
            {
                Close(() => 
                {
                    callback?.Invoke();
                });
            });

            ShowMessage(message);

            base.Open();
        }

        public void Open(string message, Action<bool> callback)
        {
            singleSection.SetActive(false);
            multipleSection.SetActive(true);

            multipleAcceptButton.onClick.RemoveAllListeners();
            multipleAcceptButton.onClick.AddListener(() =>
            {
                Close(() =>
                {
                    callback?.Invoke(true);
                });
            });

            multipleCancelButton.onClick.RemoveAllListeners();
            multipleCancelButton.onClick.AddListener(() =>
            {
                Close(() =>
                {
                    callback?.Invoke(false);
                });
            });

            ShowMessage(message);

            base.Open();
        }

        private void ShowMessage(string message)
        {
            messageText.text = LocalizationManager.instance.Translate("Message", message);

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);
        }
    }
}