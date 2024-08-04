using TMPro;
using UnityEngine;

namespace Devenant
{
    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextStyle : MonoBehaviour
    {
        public TextMeshProUGUI text { get { if(_text == null) { _text = GetComponent<TextMeshProUGUI>(); } return _text; } }
        private TextMeshProUGUI _text;

        public TextStyleDataAsset style { get { return _style; } }
        [SerializeField] private TextStyleDataAsset _style;

        private void Awake()
        {
            UpdateStyle();
        }

        public void UpdateStyle()
        {
            if (style != null)
            {
                text.font = style.font;
                text.fontSize = style.size;
                text.color = style.color;
                text.fontStyle = style.styles;
            }
            else
            {
                text.color = Color.red;
            }
        }
    }
}
