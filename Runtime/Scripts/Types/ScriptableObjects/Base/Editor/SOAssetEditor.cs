using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEditor.VersionControl;
using System;
using System.IO;
using static Codice.Client.BaseCommands.WkStatus.Printers.StatusChangeInfo;

namespace Devenant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SOAsset), true)]
    public class SOAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            if(GUILayout.Button("Setup"))
            {
                Setup((SOAsset)target);
            }

            if(GUILayout.Button("Setup All"))
            {
                SetupAll();
            }

            GUILayout.EndHorizontal();

            if(GUILayout.Button("Check required properties"))
            {
                CheckRequiredProperties();
            }
        }

        private void Setup(SOAsset asset)
        {
            string groupName = asset.GetType().Name;

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            AddressableAssetGroup group = settings.FindGroup(groupName);

            if(group == null)
            {
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            }

            string assetpath = AssetDatabase.GetAssetPath(asset);
            string guid = AssetDatabase.AssetPathToGUID(assetpath);

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group, false, false);
            List<AddressableAssetEntry> entriesAdded = new List<AddressableAssetEntry> { entry };

            entry.SetLabel(groupName, true, true, false);

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }

        private void SetupAll()
        {
            foreach(string guid in AssetDatabase.FindAssets(string.Format("t:{0}", typeof(SOAsset))))
            {
                Setup(AssetDatabase.LoadAssetAtPath<SOAsset>(AssetDatabase.GUIDToAssetPath(guid)));
            }
        }

        private void CheckRequiredProperties()
        {
            List<string> emptyProperties = new List<string>();

            foreach(string guid in AssetDatabase.FindAssets(string.Format("t:{0}", typeof(SOAsset))))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                SOAsset asset = AssetDatabase.LoadAssetAtPath<SOAsset>(path);

                SerializedObject serializedObject = new SerializedObject(asset);

                SerializedProperty serializedProperty = serializedObject.GetIterator();

                while(serializedProperty.NextVisible(true))
                {
                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference && serializedProperty.objectReferenceValue == null)
                    {
                        if(Attribute.IsDefined(asset.GetType().GetField(serializedProperty.name), typeof(RequiredAttribute)))
                        {
                            Selection.activeObject = asset;

                            emptyProperties.Add(path + ": " + serializedProperty.displayName);
                        }
                    }
                }
            }

            if(emptyProperties.Count > 0)
            {
                string message = string.Empty;

                foreach(string fieldName in emptyProperties)
                {
                    message += "- " + fieldName + "\n";
                }

                EditorUtility.DisplayDialog("Required properties empty", message, "Accept");
            }
            else
            {
                EditorUtility.DisplayDialog("Congratulations!", "All required fields are filled", "Accept");
            }
        }
    }
}
