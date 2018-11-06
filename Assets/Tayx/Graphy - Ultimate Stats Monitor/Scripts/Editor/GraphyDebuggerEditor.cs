/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            02-Jan-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Tayx.Graphy
{
    [CustomEditor(typeof(GraphyDebugger))]
    internal class GraphyDebuggerEditor : Editor
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Finish spacing on "OnInspectorGUI".
         * Add sections to "OnInspectorGUI".
         * Fix the use of Space to be consistent with "GraphyManagerEditor".
         * --------------------------------------*/
        
        #region Variables -> Private

        private GraphyDebugger  m_target;

        private int             m_newDebugPacketListSize                = 0;

        private int             m_previouslySelectedDebugPacketIndex    = 0;
        private int             m_currentlySelectedDebugPacketIndex     = 0;

        private int             m_selectedDebugPacketCondition          = 0;

        private GUISkin         m_skin;

        private GUIStyle        m_headerStyle1;
        private GUIStyle        m_headerStyle2;

        private Texture2D       m_logoTexture;

        #endregion

        #region Methods -> Unity Callbacks

        private void OnEnable()
        {
            m_target = (GraphyDebugger) target;
        }

        #endregion

        #region Methods -> Public Override

        public override void OnInspectorGUI()
        {
            if (m_target == null && target == null)
            {
                base.OnInspectorGUI();

                return;
            }

            LoadGuiStyles();

            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            float defaultFieldWidth = EditorGUIUtility.fieldWidth;

            //===== CONTENT REGION ========================================================================

            GUILayout.Space(20);

            #region Section -> Logo

            if (m_logoTexture != null)
            {
                GUILayout.Label
                (
                    image: m_logoTexture,
                    style: new GUIStyle(GUI.skin.GetStyle("Label"))
                    {
                        alignment = TextAnchor.UpperCenter
                    }
                );

                GUILayout.Space(10);
            }
            else
            {
                EditorGUILayout.LabelField
                (
                    label: "[ GRAPHY - DEBUGGER ]",
                    style: m_headerStyle1
                );
            }

            #endregion

            GUILayout.Space(5); //Extra pixels added when the logo is used.

            #region Section -> Settings

            SerializedObject serObj = serializedObject;

            SerializedProperty debugPacketList = serObj.FindProperty("m_debugPackets"); // Find the List in our script and create a refrence of it

            //Update our list
            serObj.Update();
 
            EditorGUILayout.LabelField("Current [Debug Packets] list size: " + debugPacketList.arraySize);

            EditorGUIUtility.fieldWidth = 32;
            EditorGUILayout.BeginHorizontal();

            

            m_newDebugPacketListSize = EditorGUILayout.IntField
            (
                label: "Define a new list size",
                value: m_newDebugPacketListSize
            );
            
            if (GUILayout.Button("Resize List"))
            {
                if (EditorUtility.DisplayDialog
                (
                    title:
                    "Resize List",

                    message:
                    "Are you sure you want to resize the entire List?\n\n" +
                    "Current List Size -> " +
                    debugPacketList.arraySize +
                    "\n" +
                    "New List Size -> " +
                    m_newDebugPacketListSize +
                    "\n" +
                    "This will add default entries if the value is greater than the list size, or erase the bottom values until the new size specified.",

                    ok:
                    "Resize",

                    cancel:
                    "Cancel")
                )
                {
                    m_currentlySelectedDebugPacketIndex = 0;

                    if (m_newDebugPacketListSize != debugPacketList.arraySize)
                    {
                        while (m_newDebugPacketListSize > debugPacketList.arraySize)
                        {
                            debugPacketList.InsertArrayElementAtIndex(debugPacketList.arraySize);
                            SetDefaultDebugPacketValues(debugPacketList);
                        }
                        while (m_newDebugPacketListSize < debugPacketList.arraySize)
                        {
                            debugPacketList.DeleteArrayElementAtIndex(debugPacketList.arraySize - 1);
                        }
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("NOT RECOMMENDED (Only use for first initialization)", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (debugPacketList.arraySize < 1)
            {
                m_previouslySelectedDebugPacketIndex = 0;
                m_currentlySelectedDebugPacketIndex = 0;
                m_selectedDebugPacketCondition = 0;

                serializedObject.ApplyModifiedProperties();
                return;
            }

            m_headerStyle2.contentOffset = Vector2.down * 3f;

            EditorGUILayout.LabelField("Selected debug packet:");

            EditorGUILayout.BeginHorizontal();

            List<string> debugPacketNames = new List<string>();
            for (int i = 0; i < debugPacketList.arraySize; i++)
            {
                SerializedProperty listItem = debugPacketList.GetArrayElementAtIndex(i);
                // NOTE: If the Popup detects two equal strings, it just paints 1, that's why I always add the "i"
                char checkMark = listItem.FindPropertyRelative("Active").boolValue ? '\u2714' : '\u2718';
                debugPacketNames.Add
                (
                    (i + 1) +
                    " (" +
                    checkMark +
                    ") " +
                    " - ID: " +
                    listItem.FindPropertyRelative("Id").intValue +
                    " (Conditions: " +
                    listItem.FindPropertyRelative("DebugConditions").arraySize +
                    ")"
                );
            }

            m_currentlySelectedDebugPacketIndex = EditorGUILayout.Popup(m_currentlySelectedDebugPacketIndex, debugPacketNames.ToArray());

            if (m_currentlySelectedDebugPacketIndex != m_previouslySelectedDebugPacketIndex)
            {
                m_selectedDebugPacketCondition = 0;

                m_previouslySelectedDebugPacketIndex = m_currentlySelectedDebugPacketIndex;
            }

            Color defaultGUIColor = GUI.color;

            GUI.color = new Color(0.7f, 1f, 0.0f, 1f);

            //Or add a new item to the List<> with a button

            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                debugPacketList.InsertArrayElementAtIndex(debugPacketList.arraySize);
                SetDefaultDebugPacketValues(debugPacketList);
            }

            GUI.color = new Color(1f, 0.7f, 0.0f, 1f);

            //Remove this index from the List

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                debugPacketList.DeleteArrayElementAtIndex(m_currentlySelectedDebugPacketIndex);
                if (m_currentlySelectedDebugPacketIndex > 0)
                {
                    m_currentlySelectedDebugPacketIndex--;
                }

                if (debugPacketList.arraySize < 1)
                {
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
            }

            GUI.color = defaultGUIColor;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //Display our list to the inspector window

            SerializedProperty listItemSelected = debugPacketList.GetArrayElementAtIndex(m_currentlySelectedDebugPacketIndex);

            SerializedProperty Active               = listItemSelected.FindPropertyRelative("Active");
            SerializedProperty Id                   = listItemSelected.FindPropertyRelative("Id");
            SerializedProperty ExecuteOnce          = listItemSelected.FindPropertyRelative("ExecuteOnce");
            SerializedProperty InitSleepTime        = listItemSelected.FindPropertyRelative("InitSleepTime");
            SerializedProperty ExecuteSleepTime     = listItemSelected.FindPropertyRelative("ExecuteSleepTime");
            SerializedProperty ConditionEvaluation  = listItemSelected.FindPropertyRelative("ConditionEvaluation");
            SerializedProperty DebugConditions      = listItemSelected.FindPropertyRelative("DebugConditions");
            SerializedProperty MessageType          = listItemSelected.FindPropertyRelative("MessageType");
            SerializedProperty Message              = listItemSelected.FindPropertyRelative("Message");
            SerializedProperty TakeScreenshot       = listItemSelected.FindPropertyRelative("TakeScreenshot");
            SerializedProperty ScreenshotFileName   = listItemSelected.FindPropertyRelative("ScreenshotFileName");
            SerializedProperty DebugBreak           = listItemSelected.FindPropertyRelative("DebugBreak");
            SerializedProperty UnityEvents          = listItemSelected.FindPropertyRelative("UnityEvents");

            #endregion

            EditorGUILayout.LabelField
            (
                label:
                "[ PACKET ] - ID: " +
                Id.intValue +
                " (Conditions: " +
                DebugConditions.arraySize +
                ")",

                style: m_headerStyle2
            );

            EditorGUIUtility.labelWidth = 150;
            EditorGUIUtility.fieldWidth = 35;

            Active.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text:       "Active",
                    tooltip:    "If false, it will not be checked"
                ),
                value:          Active.boolValue
            );

            Id.intValue = EditorGUILayout.IntField
            (
                new GUIContent
                (
                    text:       "ID",
                    tooltip:    "Optional Id. It's used to get or remove DebugPackets in runtime"
                ),
                value:          Id.intValue
            );

            ExecuteOnce.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text:       "Execute once",
                    tooltip:    "If true, once the actions are executed, this DebugPacket will delete itself"
                ),
                value:          ExecuteOnce.boolValue
            );

            InitSleepTime.floatValue = EditorGUILayout.FloatField
            (
                new GUIContent
                (
                    text:       "Init sleep time",
                    tooltip:    "Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game)"
                ),
                value:          InitSleepTime.floatValue
            );

            ExecuteSleepTime.floatValue = EditorGUILayout.FloatField
            (
                new GUIContent
                (
                    text:       "Sleep time after execute",
                    tooltip:    "Time to wait before checking if conditions are met again (once they have already been met and if ExecuteOnce is false)"
                ),
                value:          ExecuteSleepTime.floatValue
            );

            

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("[ CONDITIONS ] (" + DebugConditions.arraySize + ")", m_headerStyle2);

            EditorGUILayout.PropertyField
            (
                ConditionEvaluation,
                new GUIContent("Condition evaluation")
            );

            EditorGUILayout.Space();
            
            if (DebugConditions.arraySize < 1)
            {
                DebugConditions.InsertArrayElementAtIndex(DebugConditions.arraySize);
                m_selectedDebugPacketCondition = 0;
            }

            EditorGUILayout.BeginHorizontal();

            List<string> debugPacketConditionNames = new List<string>();
            for (int i = 0; i < DebugConditions.arraySize; i++)
            {
                SerializedProperty listItem = DebugConditions.GetArrayElementAtIndex(i);
                // NOTE: If the Popup detects two equal strings, it just paints 1, that's why I always add the "i"

                string conditionName = (i + 1).ToString() + " - ";
                conditionName += GetComparerStringFromDebugVariable((GraphyDebugger.DebugVariable)listItem.FindPropertyRelative("Variable").intValue) + " ";
                conditionName += GetComparerStringFromDebugComparer((GraphyDebugger.DebugComparer)listItem.FindPropertyRelative("Comparer").intValue) + " ";
                conditionName += listItem.FindPropertyRelative("Value").floatValue.ToString();
                
                debugPacketConditionNames.Add(conditionName);
            }

            m_selectedDebugPacketCondition = EditorGUILayout.Popup(m_selectedDebugPacketCondition, debugPacketConditionNames.ToArray());

            GUI.color = new Color(0.7f, 1f, 0.0f, 1f);

            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                DebugConditions.InsertArrayElementAtIndex(DebugConditions.arraySize);
            }

            if (DebugConditions.arraySize > 1)
            {
                GUI.color = new Color(1f, 0.7f, 0.0f, 1f);
            }
            else
            {
                GUI.color = new Color(1f, 0.7f, 0.0f, 0.5f);
            }

            //Remove this index from the List
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                if (DebugConditions.arraySize > 1)
                {
                    DebugConditions.DeleteArrayElementAtIndex(m_selectedDebugPacketCondition);
                    if (m_selectedDebugPacketCondition > 0)
                    {
                        m_selectedDebugPacketCondition--;
                    }
                }
            }

            GUI.color = defaultGUIColor;

            EditorGUILayout.EndHorizontal();

            SerializedProperty conditionListItemSelected = DebugConditions.GetArrayElementAtIndex(m_selectedDebugPacketCondition);

            SerializedProperty Variable = conditionListItemSelected.FindPropertyRelative("Variable");
            SerializedProperty Comparer = conditionListItemSelected.FindPropertyRelative("Comparer");
            SerializedProperty Value    = conditionListItemSelected.FindPropertyRelative("Value");

            EditorGUILayout.PropertyField
            (
                Variable,
                new GUIContent("Variable")
            );

            EditorGUILayout.PropertyField
            (
                Comparer,
                new GUIContent("Comparer")
            );

            EditorGUILayout.PropertyField
            (
                Value,
                new GUIContent("Value")
            );

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("[ ACTIONS ]", m_headerStyle2);

            EditorGUIUtility.labelWidth = 140;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                MessageType,
                new GUIContent("Message type")
            );

            EditorGUILayout.PropertyField(Message);

            TakeScreenshot.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text:       "Take screenshot",
                    tooltip:    "If true, it takes a screenshot and stores it. The location where the image is written to can include a directory/folder list. With no directory/folder list the image will be written into the Project folder. On mobile platforms the filename is appended to the persistent data path."
                ),
                value:          TakeScreenshot.boolValue
            );

            if (TakeScreenshot.boolValue)
            {
                EditorGUILayout.PropertyField
                (
                    ScreenshotFileName,
                    new GUIContent
                    (
                        text: "Screenshot file name",
                        tooltip: "Avoid this characters: * . \" / \\ [ ] : ; | = , \n\nIt will have the date appended at the end to avoid overwriting."
                    )
                );
            }

            DebugBreak.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text: "Debug Break",
                    tooltip: "If true, it pauses the editor"
                ),
                DebugBreak.boolValue
            );
            
            EditorGUILayout.PropertyField(UnityEvents);

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            serializedObject.ApplyModifiedProperties();
            
        }

        #endregion

        #region Methods -> Private

        private void LoadGuiStyles()
        {
            string path = GetMonoScriptFilePath(this);

            path = path.Split(new string[] { "Assets" }, StringSplitOptions.None)[1]
                       .Split(new string[] { "Tayx"   }, StringSplitOptions.None)[0];

            m_logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                "Assets" +
                path +
                "Tayx/Graphy - Ultimate Stats Monitor/Textures/Debugger_Logo_" +
                (EditorGUIUtility.isProSkin ? "White.png" : "Dark.png")
            );

            m_skin = AssetDatabase.LoadAssetAtPath<GUISkin>
            (
                "Assets" +
                path +
                "Tayx/Graphy - Ultimate Stats Monitor/GUI/Graphy.guiskin"
            );

            if (m_skin != null)
            {
                m_headerStyle1 = m_skin.GetStyle("Header1");
                m_headerStyle2 = m_skin.GetStyle("Header2");

                SetGuiStyleFontColor
                (
                    guiStyle: m_headerStyle2,
                    color: EditorGUIUtility.isProSkin ? Color.white : Color.black
                );
            }
            else
            {
                m_headerStyle1 = EditorStyles.boldLabel;
                m_headerStyle2 = EditorStyles.boldLabel;
            }
        }

        private void SetGuiStyleFontColor(GUIStyle guiStyle, Color color)
        {
            guiStyle.normal     .textColor = color;
            guiStyle.hover      .textColor = color;
            guiStyle.active     .textColor = color;
            guiStyle.focused    .textColor = color;
            guiStyle.onNormal   .textColor = color;
            guiStyle.onHover    .textColor = color;
            guiStyle.onActive   .textColor = color;
            guiStyle.onFocused  .textColor = color;
        }

        private string GetMonoScriptFilePath(ScriptableObject scriptableObject)
        {
            MonoScript ms   = MonoScript.FromScriptableObject(scriptableObject);

            string filePath = AssetDatabase.GetAssetPath(ms);

            FileInfo fi     = new FileInfo(filePath);

            if (fi.Directory != null)
            {
                filePath = fi.Directory.ToString();

                return filePath.Replace
                (
                    oldChar: '\\',
                    newChar: '/'
                );
            }
            return null;
            
        }

        private void SetDefaultDebugPacketValues(SerializedProperty debugPacketSerializedProperty)
        {
            GraphyDebugger.DebugPacket debugPacket = new GraphyDebugger.DebugPacket();

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("Active")
                .boolValue  = debugPacket.Active;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("Id")
                .intValue   = debugPacketSerializedProperty.arraySize;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("ExecuteOnce")
                .boolValue  = debugPacket.ExecuteOnce;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("InitSleepTime")
                .floatValue = debugPacket.InitSleepTime;

            debugPacketSerializedProperty.GetArrayElementAtIndex(debugPacketSerializedProperty.arraySize - 1)
                .FindPropertyRelative("ExecuteSleepTime")
                .floatValue = debugPacket.ExecuteSleepTime;
        }

        private string GetComparerStringFromDebugVariable(GraphyDebugger.DebugVariable debugVariable)
        {
            switch (debugVariable)
            {
                case GraphyDebugger.DebugVariable.Fps:
                    return "FPS Current";
                case GraphyDebugger.DebugVariable.Fps_Min:
                    return "FPS Min";
                case GraphyDebugger.DebugVariable.Fps_Max:
                    return "FPS Max";
                case GraphyDebugger.DebugVariable.Fps_Avg:
                    return "FPS Avg";

                case GraphyDebugger.DebugVariable.Ram_Allocated:
                    return "Ram Allocated";
                case GraphyDebugger.DebugVariable.Ram_Reserved:
                    return "Ram Reserved";
                case GraphyDebugger.DebugVariable.Ram_Mono:
                    return "Ram Mono";

                case GraphyDebugger.DebugVariable.Audio_DB:
                    return "Audio DB";

                default:
                    return null;

            }
        }

        private string GetComparerStringFromDebugComparer(GraphyDebugger.DebugComparer debugComparer)
        {
            switch (debugComparer)
            {
                case GraphyDebugger.DebugComparer.Less_than:
                    return "<";
                case GraphyDebugger.DebugComparer.Equals_or_less_than:
                    return "<=";
                case GraphyDebugger.DebugComparer.Equals:
                    return "==";
                case GraphyDebugger.DebugComparer.Equals_or_greater_than:
                    return ">=";
                case GraphyDebugger.DebugComparer.Greater_than:
                    return ">";

                default:
                    return null;
            }
        }

        #endregion
    }
}