using UnityEditor;
using UnityEngine;

namespace Tayx.Graphy
{
    public class GraphyMenuItem
    {
        [MenuItem("Tools/Graphy/Create Prefab Variant")]
        static void CreatePrefabVariant()
        {
            // Directory checking
            if (!AssetDatabase.IsValidFolder("Assets/Graphy - Ultimate Stats Monitor"))
            {
                AssetDatabase.CreateFolder("Assets", "Graphy - Ultimate Stats Monitor");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Graphy - Ultimate Stats Monitor/Prefab")) {
                AssetDatabase.CreateFolder("Assets/Graphy - Ultimate Stats Monitor", "Prefab");
            }

            Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Packages/com.tayx.graphy/Prefab/[Graphy].prefab", typeof(GameObject));
            GameObject objectSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
            GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objectSource, "Assets/Graphy - Ultimate Stats Monitor/Prefab/[Graphy].prefab");

            Object.DestroyImmediate(objectSource);

            foreach(SceneView scene in SceneView.sceneViews)
            {
                scene.ShowNotification(new GUIContent("Prefab Variant Created at Assets/Graphy - Ultimate Stats Monitor/Prefab"));
            }
        }
        [MenuItem("Tools/Graphy/Import Graphy Customizer Scene")]
        static void ImportGraphy()
        {
#if GRAPHY_NEW_INPUT
            AssetDatabase.ImportPackage("Packages/com.tayx.graphy/Customize Scene New Input System.unitypackage", true);
#else
            AssetDatabase.ImportPackage("Packages/com.tayx.graphy/Customize Scene.unitypackage", true);
#endif
        }
    }
}