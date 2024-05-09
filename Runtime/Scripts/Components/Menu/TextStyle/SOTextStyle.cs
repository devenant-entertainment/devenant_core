using UnityEngine;
using TMPro;

namespace Devenant
{
    [CreateAssetMenu(fileName = "text_style_", menuName = "Devenant/Core/Text/Style", order = 0)]
    public class SOTextStyle : ScriptableObject
    {
        public TMP_FontAsset font;
        public int size;
        public Color color;
        public FontStyles styles;
    }
}
