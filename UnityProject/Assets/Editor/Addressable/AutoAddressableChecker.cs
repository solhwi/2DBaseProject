using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AutoAddressableChecker : AssetPostprocessor
{
    public static Dictionary<ResourceType, string[]> assetExtensionFilterDictionary = new Dictionary<ResourceType, string[]>()
    {
        { ResourceType.Material, new string[]{ ".mat"} },
        { ResourceType.Prefab, new string[]{ ".prefab" } },
        { ResourceType.Scene, new string[]{ ".unity", } },
        { ResourceType.Animation, new string[]{ ".anim", ".controller",} },
        { ResourceType.UnityAsset, new string[]{ ".asset" } },
    };

    private static readonly string DefaultGroupName = "Default Local Group";

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (importedAssets == null ||
            importedAssets.Length <= 0)
            return;

        foreach (var importedAsset in importedAssets)
        {
            string assetExtension = Path.GetExtension(importedAsset);

            foreach(var extensionPair in assetExtensionFilterDictionary)
            {
                var labelName = extensionPair.Key;
                var extensionFilters = extensionPair.Value;

                if (extensionFilters.Contains(assetExtension))
                {
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(importedAsset);
                    AddressableHelper.CreateAssetEntry<Object>(obj, DefaultGroupName, labelName.ToString());

                    break;
                }
            }
        }  
    }
}
