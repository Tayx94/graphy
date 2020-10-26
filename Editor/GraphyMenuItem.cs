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

            if (!AssetDatabase.IsValidFolder("Assets/Graphy - Ultimate Stats Monitor/Prefab"))
            {
                AssetDatabase.CreateFolder("Assets/Graphy - Ultimate Stats Monitor", "Prefab");
            }

            string graphyPrefabGuid = AssetDatabase.FindAssets( "[Graphy]" )[ 0 ];

            Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( graphyPrefabGuid ), typeof(GameObject));
            GameObject objectSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
            GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objectSource, "Assets/Graphy - Ultimate Stats Monitor/Prefab/[Graphy].prefab");

            Object.DestroyImmediate(objectSource);

            foreach(SceneView scene in SceneView.sceneViews)
            {
                scene.ShowNotification(new GUIContent( "Prefab Variant Created at \"Assets/Graphy - Ultimate Stats Monitor/Prefab\"!" ) );
            }
        }

        [MenuItem("Tools/Graphy/Import Graphy Customization Scene")]
        static void ImportGraphyCustomizationScene()
        {
            string customizationSceneGuid = AssetDatabase.FindAssets( "Graphy_CustomizationScene" )[ 0 ];

            AssetDatabase.ImportPackage( AssetDatabase.GUIDToAssetPath( customizationSceneGuid ), true);
        }
    }
}