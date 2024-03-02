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
        [SerializeField] private GameObject achievementElement;
        
        [SerializeField] private Button closeButton;

        private Content achievementContent;

        public override void Open(Action callback = null)
        {
            achievementContent?.Clear();

            achievementContent = new Content(achievementHolder, achievementElement);

            List<Achievement> sortedAchievements = AchievementManager.instance.achievements.ToList();
            sortedAchievements.Sort((a, b) => a.completed.CompareTo(b.completed));

            foreach(Achievement achievement in sortedAchievements)
            {
                Create(achievement);
            }

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            base.Open(callback);
        }

        private void Create(Achievement achievement)
        {
            GameObject newAchievement = achievementContent.Create();

            newAchievement.transform.Find("IconImage").GetComponent<Image>().sprite = achievement.icon; 
            newAchievement.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = LocalizationManager.instance.Translate("achievement", achievement.name + "_name");
            newAchievement.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = LocalizationManager.instance.Translate("achievement", achievement.name + "_description");

            newAchievement.transform.Find("Completed").gameObject.SetActive(achievement.completed);

            newAchievement.transform.Find("ProgressBar").gameObject.SetActive(achievement.maxValue > 1);
            newAchievement.transform.Find("ProgressBar/ProgressImage").GetComponent<Image>().fillAmount = (float)achievement.value / (float)achievement.maxValue;
            newAchievement.transform.Find("ProgressBar/ProgressText").GetComponent<TextMeshProUGUI>().text = achievement.value.ToString() + "/" + achievement.maxValue.ToString();

        }
    }
}
