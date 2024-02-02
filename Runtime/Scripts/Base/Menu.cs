using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public interface IMenu
    {
        public void Open(Action callback = null);
        public void Close(Action callback = null);
    }

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Menu<T> : MonoBehaviour, IMenu where T : Menu<T>
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

        private void OnEnable()
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

        public class Group
        {
            public readonly IMenu[] tabs;

            public Group(IMenu[] tabs, Action callback = null)
            {
                this.tabs = tabs;
            }

            public void Open(int index, Action callback = null)
            {
                for(int i = 0; i < tabs.Length; i ++)
                {
                    if (i == index)
                    {
                        tabs[i].Open(callback);
                    }
                    else
                    {
                        tabs[i].Close();
                    }
                }
            }
        }

        public class Content
        {
            private RectTransform holder;
            private GameObject reference;

            private List<GameObject> elements;

            public Content(RectTransform holder, GameObject reference)
            {
                this.holder = holder;
                this.reference = reference;

                elements = new List<GameObject>();

                reference.SetActive(false);
            }

            public GameObject Create()
            {
                GameObject newElement = Instantiate(reference, holder);
                newElement.SetActive(true);

                elements.Add(newElement);

                return newElement;
            }

            public void Remove(GameObject element)
            {
                elements.Remove(element);

                Destroy(element);
            }

            public void Clear()
            {
                foreach(GameObject element in elements)
                {
                    Destroy(element);
                }

                elements = new List<GameObject>();
            }
        }
    }
}