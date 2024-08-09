using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class InitialLoadingMenu : Menu<InitialLoadingMenu>
    {
        [SerializeField] private Slider loadingBar;

        private Coroutine currentCoroutine;

        public override void Open(Action callback = null)
        {
            loadingBar.value = 0;

            base.Open(callback);
        }

        public override void Close(Action callback = null)
        {
            loadingBar.value = 1;

            base.Close(callback);
        }

        public void SetValue(float value)
        {
            FadeValue(loadingBar, Mathf.Clamp01(value), 0.2f);
        }

        private void FadeValue(Slider target, float value, float time, Action callback = null)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);

                currentCoroutine = null;
            }

            currentCoroutine = instance.StartCoroutine(FadeCoroutine());

            IEnumerator FadeCoroutine()
            {
                float startAlpha = target.value;
                float elapsedTime = 0f;

                while (elapsedTime < time)
                {
                    target.value = Mathf.Lerp(startAlpha, value, elapsedTime / time);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                target.value = value;

                callback?.Invoke();
            }
        }
    }
}
