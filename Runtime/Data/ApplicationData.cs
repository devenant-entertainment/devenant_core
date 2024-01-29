using UnityEngine;

namespace Devenant
{
    [CreateAssetMenu(fileName = "ApplicationData", menuName = "Devenant/Application Data")]
    public class ApplicationData : ScriptableObject
    {
        public string infoUrl;
        public PlatformValue<string> storeUrl;
    }
}
