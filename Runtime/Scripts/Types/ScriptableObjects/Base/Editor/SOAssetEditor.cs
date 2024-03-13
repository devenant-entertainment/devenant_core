using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEditor.VersionControl;

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

            if(GUILayout.Button("Setup"))
            {
                Setup((SOAsset)target);
            }

            if(GUILayout.Button("Setup All"))
            {
                SetupAll();
            }
        }

        private void SetupAll()
        {
            foreach(string guid in AssetDatabase.FindAssets(string.Format("t:{0}", typeof(SOAsset))))
            {
                Setup(AssetDatabase.LoadAssetAtPath<SOAsset>(AssetDatabase.GUIDToAssetPath(guid)));
            }
        }

        public void Setup(SOAsset asset)
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
    }
}
