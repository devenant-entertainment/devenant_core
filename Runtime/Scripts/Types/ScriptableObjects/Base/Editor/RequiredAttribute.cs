using UnityEditor;
using UnityEngine;

namespace Devenant
{
    public class RequiredAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if(property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {
                GUIStyle warningStyle = new GUIStyle(GUI.skin.label);

                warningStyle.alignment = TextAnchor.MiddleRight;
                warningStyle.normal.textColor = Color.red;

                position.width -= 20;

                EditorGUI.LabelField(position, "Required", warningStyle);
            }
        }
    }
#endif
}
