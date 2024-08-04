using System;
using System.Collections;
using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Menu<T> : MonoBehaviour where T : Menu<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }

                return _instance;
            }
        }
        private static T _instance;

        public RectTransform rectTransform { get { if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }
        private RectTransform _rectTransform;

        public CanvasGroup canvasGroup { get { if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>(); return _canvasGroup; } }
        private CanvasGroup _canvasGroup;

        public bool isOpen { get { return _isOpen; } private set { _isOpen = value; } }
        private bool _isOpen = false;

        protected void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public virtual void Open(Action callback = null)
        {
            if(!isOpen)
            {
                isOpen = true;

                rectTransform.SetAsLastSibling();

                canvasGroup.blocksRaycasts = true;

                Fade(canvasGroup, 1, 0.25f, () =>
                {
                    canvasGroup.interactable = true;

                    callback?.Invoke();
                });
            }
        }

        public virtual void Close(Action callback = null)
        {
            if(isOpen)
            {
                canvasGroup.interactable = false;

                Fade(canvasGroup, 0, 0.25f, () =>
                {
                    canvasGroup.blocksRaycasts = false;

                    rectTransform.SetAsFirstSibling();

                    isOpen = false;

                    callback?.Invoke();
                });
            }
        }

        protected void Fade(CanvasGroup target, float value, float time, Action callback)
        {
            instance.StartCoroutine(FadeCoroutine());

            IEnumerator FadeCoroutine()
            {
                float startAlpha = target.alpha;
                float elapsedTime = 0f;

                while(elapsedTime < time)
                {
                    target.alpha = Mathf.Lerp(startAlpha, value, elapsedTime / time);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                target.alpha = value;

                callback?.Invoke();
            }
        }
    }
}