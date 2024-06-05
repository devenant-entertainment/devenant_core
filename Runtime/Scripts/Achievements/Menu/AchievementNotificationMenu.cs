using Devenant.Menu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class AchievementNotificationMenu : Menu<AchievementNotificationMenu>
    {
        [SerializeField] private float openTime = 3;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;

        private List<Achievement> achievements = new List<Achievement>();

        private void OnEnable()
        {
            AchievementManager.onCompleted += OnAchievementCompleted;
        }

        private void OnDisable()
        {
            AchievementManager.onCompleted -= OnAchievementCompleted;
        }

        private void OnAchievementCompleted(Achievement achievement)
        {
            achievements.Add(achievement);

            if(!isOpen)
            {
                Show();
            }
        }

        private void Show()
        {
            iconImage.sprite = achievements[0].icon;
            nameText.text = LocalizationSettingsManager.instance.Translate("Achievement", achievements[0].name + "_name");

            base.Open(() =>
            {
                StartCoroutine(CloseCoroutine());
            });

            IEnumerator CloseCoroutine()
            {
                yield return new WaitForSeconds(openTime);

                Close(() =>
                {
                    achievements.RemoveAt(0);

                    if(achievements.Count > 0)
                    {
                        Show();
                    }
                });
            }
        }
    }
}
