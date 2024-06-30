using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // Ensure this namespace is included

public class CreateAssetBundle : MonoBehaviour
{
    // Add the MenuItem attribute to create a menu item in the Unity Editor
    [MenuItem("Assets/Build All Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        // Define the output path for the asset bundles
        string assetBundleDirectory = "Assets/AssetBundles";

        // Create the directory if it doesn't exist
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }

        // Build the asset bundles
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);

        // Optional: Refresh the AssetDatabase to show the new asset bundles in the Project window
        AssetDatabase.Refresh();
    }
}
