using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Devenant
{
    public class AchievementMenuElement : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private GameObject completedIndicator;
        [SerializeField] private GameObject progressIndicator;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI progressText;

        public void Setup(AchievementData achievement)
        {
            iconImage.sprite = achievement.icon;
            nameText.text = LocalizationManager.instance.Translate("Achievement", achievement.name + "_name");
            descriptionText.text = LocalizationManager.instance.Translate("Achievement", achievement.name + "_description");

            completedIndicator.SetActive(achievement.completed);

            progressIndicator.SetActive(achievement.maxValue > 1);
            progressBar.fillAmount = (float)achievement.value / (float)achievement.maxValue;
            progressText.text = achievement.value.ToString() + "/" + achievement.maxValue.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
