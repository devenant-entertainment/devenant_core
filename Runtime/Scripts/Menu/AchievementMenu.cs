using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class AchievementMenu : Menu<AchievementMenu>
    {
        [SerializeField] private RectTransform achievementHolder;
        [SerializeField] private AchievementMenuElement achievementElement;
        
        [SerializeField] private Button closeButton;

        private Content achievementContent;

        public override void Open(Action callback = null)
        {
            achievementContent?.Clear();

            achievementContent = new Content(achievementHolder, achievementElement.gameObject);

            List<AchievementData> sortedAchievements = AchievementManager.instance.achievements.Get().ToList();
            sortedAchievements.Sort((a, b) => a.completed.CompareTo(b.completed));

            foreach(AchievementData achievement in sortedAchievements)
            {
                achievementContent.Create().GetComponent<AchievementMenuElement>().Setup(achievement);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(achievementHolder);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open(callback);
        }
    }
}
