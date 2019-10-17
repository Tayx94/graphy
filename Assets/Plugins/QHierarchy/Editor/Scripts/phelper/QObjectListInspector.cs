using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pdata;

namespace qtools.qhierarchy.phelper
{
    [CustomEditor(typeof(QObjectList))]
    public class QObjectListInspector : Editor
    {
    	public override void OnInspectorGUI()
    	{
    		EditorGUILayout.HelpBox("\nThis is an auto created GameObject that managed by QHierarchy.\n\n" + 
                                    "It stores references to some GameObjects in the current scene. This object will not be included in the application build.\n\n" + 
                                    "You can safely remove it, but lock / unlock / visible / etc. states will be reset. Delete this object if you want to remove the QHierarchy.\n\n" +
                                    "This object can be hidden if you uncheck \"Show QHierarchy GameObject\" in the settings of the QHierarchy.\n"
                                    , MessageType.Info, true);

            if (QSettings.getInstance().get<bool>(QSetting.AdditionalShowObjectListContent))
            {
                if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(20)), "Hide content"))
                {
                    QSettings.getInstance().set(QSetting.AdditionalShowObjectListContent, false);
                }
                base.OnInspectorGUI();
            }
            else
            {
                if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(20)), "Show content"))
                {
                    QSettings.getInstance().set(QSetting.AdditionalShowObjectListContent, true);
                }
            }
    	}
    }
}