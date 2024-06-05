#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Devenant
{
    [CustomEditor(typeof(TextStyle))]
    [CanEditMultipleObjects]
    public class TextStyleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if(GUILayout.Button("Check Text Styles"))
            {
                CheckTextStyles();
            }

            if(GUILayout.Button("Update All"))
            {
                UpdateAll();
            }

            TextStyle textStyle = (TextStyle)target;

            textStyle.UpdateStyle();
        }

        private void CheckTextStyles()
        {
            TextStyle[] textStyles = FindObjectsOfType<TextStyle>();

            for(int i = 0; i < textStyles.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Updating all text styles", textStyles[i].name, i / textStyles.Length);

                if(textStyles[i].style == null)
                {
                    if (EditorUtility.DisplayDialog("ERROR", textStyles[i].name + " styles not found.", "Find error", "Continue"))
                    {
                        Selection.activeGameObject = textStyles[i].gameObject;

                        EditorUtility.ClearProgressBar();

                        return;
                    }
                }
            }

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog("Done!", "Text styles updated successfully.", "Accept");
        }

        private void UpdateAll()
        {
            TextStyle[] textStyles = FindObjectsOfType<TextStyle>();

            for(int i = 0; i < textStyles.Length; i ++)
            {
                EditorUtility.DisplayProgressBar("Updating all text styles", textStyles[i].name, i / textStyles.Length);

                textStyles[i].UpdateStyle();
            }

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog("Done!", "Text styles updated successfully.", "Accept");
        }
    }
}
#endif