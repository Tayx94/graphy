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

            if (!AssetDatabase.IsValidFolder( "Assets/Graphy - Ultimate Stats Monitor/Prefab Variants" ) )
            {
                AssetDatabase.CreateFolder("Assets/Graphy - Ultimate Stats Monitor", "Prefab Variants" );
            }

            string graphyPrefabGuid = AssetDatabase.FindAssets( "[Graphy]" )[ 0 ];

            Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( graphyPrefabGuid ), typeof(GameObject));
            GameObject objectSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;

            int prefabVariantCount =
                AssetDatabase.FindAssets( "Graphy_Variant", new []{ "Assets/Graphy - Ultimate Stats Monitor/Prefab Variants" } ).Length;

            GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(objectSource, $"Assets/Graphy - Ultimate Stats Monitor/Prefab Variants/Graphy_Variant_{prefabVariantCount}.prefab" );

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