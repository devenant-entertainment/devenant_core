using UnityEditor;
using UnityEngine;

namespace Devenant
{
    public class RequiredAttribute : PropertyAttribute
    {
        public static bool ValidateProperty(SerializedProperty serializedProperty)
        {
            switch(serializedProperty.propertyType)
            {
                case SerializedPropertyType.ObjectReference:

                    if(serializedProperty.objectReferenceValue == null)
                    {
                        return false;
                    }

                    break;

                case SerializedPropertyType.String:

                    if(string.IsNullOrEmpty(serializedProperty.stringValue))
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if(!RequiredAttribute.ValidateProperty(property))
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
