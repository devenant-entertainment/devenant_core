using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class AchievementMenu : Menu<AchievementMenu>
    {
        [SerializeField] private RectTransform achievementHolder;
        [SerializeField] private AchievementMenuElement achievementElement;
        
        [SerializeField] private Button closeButton;

        private MenuContent achievementContent;

        public override void Open(Action callback = null)
        {
            achievementContent?.Clear();

            achievementContent = new MenuContent(achievementHolder, achievementElement.gameObject);

            List<Achievement> sortedAchievements = AchievementManager.instance.achievements.Get().ToList();
            sortedAchievements.Sort((a, b) => a.completed.CompareTo(b.completed));

            foreach(Achievement achievement in sortedAchievements)
            {
                achievementContent.Create().GetComponent<AchievementMenuElement>().Setup(achievement);
            }

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open(callback);
        }
    }
}
