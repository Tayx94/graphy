using UnityEditor;

namespace Tayx.Graphy
{
    public class GraphyMenuItem
    {
        [MenuItem("Graphy/Import Graphy")]
        static void ImportGraphy()
        {
            AssetDatabase.ImportPackage("Packages/com.tayx.graphy/Import Graphy.unitypackage", true);
        }
    }
}