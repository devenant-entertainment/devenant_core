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

        private List<AchievementData> achievements = new List<AchievementData>();

        private void OnEnable()
        {
            AchievementManager.onCompleted += OnAchievementCompleted;
        }

        private void OnDisable()
        {
            AchievementManager.onCompleted -= OnAchievementCompleted;
        }

        private void OnAchievementCompleted(AchievementData achievement)
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
            nameText.text = LocalizationManager.instance.Translate("Achievement", achievements[0].name + "_name");

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
