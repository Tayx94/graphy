/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            20-Dec-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Tayx.Graphy
{
    [CustomEditor(typeof(GraphyManager))]
    internal class GraphyManagerEditor : Editor
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * --------------------------------------*/

        #region Variables -> Private

        private GraphyManager m_target;

        private int[] m_spectrumSizeValues =
        {
            128,
            256,
            512,
            1024,
            2048,
            4096,
            8192
        };

        #region Section -> Settings

        private SerializedProperty m_graphyMode;

        private SerializedProperty m_enableOnStartup;

        private SerializedProperty m_keepAlive;

        private SerializedProperty m_background;
        private SerializedProperty m_backgroundColor;

        private SerializedProperty m_enableHotkeys;

        private SerializedProperty m_toggleModeKeyCode;
        private SerializedProperty m_toggleModeCtrl;
        private SerializedProperty m_toggleModeAlt;

        private SerializedProperty m_toggleActiveKeyCode;
        private SerializedProperty m_toggleActiveCtrl;
        private SerializedProperty m_toggleActiveAlt;


        private SerializedProperty m_graphModulePosition;

        #endregion

        #region Section -> FPS

        private bool m_fpsModuleInspectorToggle = true;

        private SerializedProperty m_fpsModuleState;

        private SerializedProperty m_goodFpsColor;
        private SerializedProperty m_goodFpsThreshold;

        private SerializedProperty m_cautionFpsColor;
        private SerializedProperty m_cautionFpsThreshold;

        private SerializedProperty m_criticalFpsColor;

        private SerializedProperty m_fpsGraphResolution;

        private SerializedProperty m_fpsTextUpdateRate;

        #endregion

        #region Section -> RAM

        private bool m_ramModuleInspectorToggle = true;

        private SerializedProperty m_ramModuleState;

        private SerializedProperty m_allocatedRamColor;
        private SerializedProperty m_reservedRamColor;
        private SerializedProperty m_monoRamColor;

        private SerializedProperty m_ramGraphResolution;

        private SerializedProperty m_ramTextUpdateRate;

        #endregion

        #region Section -> Audio

        private bool m_audioModuleInspectorToggle = true;

        private SerializedProperty m_findAudioListenerInCameraIfNull;

        private SerializedProperty m_audioListener;

        private SerializedProperty m_audioModuleState;

        private SerializedProperty m_audioGraphColor;

        private SerializedProperty m_audioGraphResolution;

        private SerializedProperty m_audioTextUpdateRate;

        private SerializedProperty m_FFTWindow;

        private SerializedProperty m_spectrumSize;

        #endregion

        #region Section -> Advanced Settings

        private bool m_advancedModuleInspectorToggle = true;

        private SerializedProperty m_advancedModulePosition;

        private SerializedProperty m_advancedModuleState;

        #endregion

        #endregion

        #region Methods -> Unity Callbacks

        private void OnEnable()
        {
            m_target = (GraphyManager)target;

            SerializedObject serObj = serializedObject;

            #region Section -> Settings

            m_graphyMode = serObj.FindProperty("m_graphyMode");

            m_enableOnStartup = serObj.FindProperty("m_enableOnStartup");

            m_keepAlive = serObj.FindProperty("m_keepAlive");

            m_background = serObj.FindProperty("m_background");
            m_backgroundColor = serObj.FindProperty("m_backgroundColor");

            m_enableHotkeys = serObj.FindProperty("m_enableHotkeys");

            m_toggleModeKeyCode = serObj.FindProperty("m_toggleModeKeyCode");

            m_toggleModeCtrl = serObj.FindProperty("m_toggleModeCtrl");
            m_toggleModeAlt = serObj.FindProperty("m_toggleModeAlt");

            m_toggleActiveKeyCode = serObj.FindProperty("m_toggleActiveKeyCode");

            m_toggleActiveCtrl = serObj.FindProperty("m_toggleActiveCtrl");
            m_toggleActiveAlt = serObj.FindProperty("m_toggleActiveAlt");

            m_graphModulePosition = serObj.FindProperty("m_graphModulePosition");

            #endregion

            #region Section -> FPS

            m_fpsModuleState = serObj.FindProperty("m_fpsModuleState");

            m_goodFpsColor = serObj.FindProperty("m_goodFpsColor");
            m_goodFpsThreshold = serObj.FindProperty("m_goodFpsThreshold");

            m_cautionFpsColor = serObj.FindProperty("m_cautionFpsColor");
            m_cautionFpsThreshold = serObj.FindProperty("m_cautionFpsThreshold");

            m_criticalFpsColor = serObj.FindProperty("m_criticalFpsColor");

            m_fpsGraphResolution = serObj.FindProperty("m_fpsGraphResolution");

            m_fpsTextUpdateRate = serObj.FindProperty("m_fpsTextUpdateRate");

            #endregion

            #region Section -> RAM

            m_ramModuleState = serObj.FindProperty("m_ramModuleState");

            m_allocatedRamColor = serObj.FindProperty("m_allocatedRamColor");
            m_reservedRamColor = serObj.FindProperty("m_reservedRamColor");
            m_monoRamColor = serObj.FindProperty("m_monoRamColor");

            m_ramGraphResolution = serObj.FindProperty("m_ramGraphResolution");

            m_ramTextUpdateRate = serObj.FindProperty("m_ramTextUpdateRate");

            #endregion

            #region Section -> Audio

            m_findAudioListenerInCameraIfNull = serObj.FindProperty("m_findAudioListenerInCameraIfNull");

            m_audioListener = serObj.FindProperty("m_audioListener");

            m_audioModuleState = serObj.FindProperty("m_audioModuleState");

            m_audioGraphColor = serObj.FindProperty("m_audioGraphColor");

            m_audioGraphResolution = serObj.FindProperty("m_audioGraphResolution");

            m_audioTextUpdateRate = serObj.FindProperty("m_audioTextUpdateRate");

            m_FFTWindow = serObj.FindProperty("m_FFTWindow");

            m_spectrumSize = serObj.FindProperty("m_spectrumSize");

            #endregion

            #region Section -> Advanced Settings

            m_advancedModulePosition = serObj.FindProperty("m_advancedModulePosition");

            m_advancedModuleState = serObj.FindProperty("m_advancedModuleState");

            #endregion

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

            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            float defaultFieldWidth = EditorGUIUtility.fieldWidth;

            //===== CONTENT REGION ========================================================================

            GUILayout.Space(20);

            #region Section -> Logo

            if (GraphyEditorStyle.ManagerLogoTexture != null)
            {
                GUILayout.Label
                (
                    image: GraphyEditorStyle.ManagerLogoTexture,
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
                    label: "[ GRAPHY - MANAGER ]",
                    style: GraphyEditorStyle.HeaderStyle1
                );
            }

            #endregion

            GUILayout.Space(5); //Extra pixels added when the logo is used.

            #region Section -> Settings

            EditorGUIUtility.labelWidth = 130;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                m_graphyMode,
                new GUIContent
                (
                    text: "Graphy Mode",
                    tooltip: "LIGHT mode increases compatibility with mobile and older, less powerful GPUs, but reduces the maximum graph resolutions to 128."
                )
            );

            GUILayout.Space(10);

            m_enableOnStartup.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text: "Enable On Startup",
                    tooltip: "If ticked, Graphy will be displayed by default on startup, otherwise it will initiate and hide."
                ),
                value: m_enableOnStartup.boolValue
            );

            // This is a neat trick to hide Graphy in the Scene if it's going to be deactivated in play mode so that it doesn't use screen space.
            if (!Application.isPlaying)
            {
                m_target.GetComponent<Canvas>().enabled = m_enableOnStartup.boolValue;
            }

            m_keepAlive.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text: "Keep Alive",
                    tooltip: "If ticked, it will survive scene changes.\n\nCAREFUL, if you set Graphy as a child of another GameObject, the root GameObject will also survive scene changes. If you want to avoid that put Graphy in the root of the Scene as its own entity."
                ),
                value: m_keepAlive.boolValue
            );

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            m_background.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text: "Background",
                    tooltip: "If ticked, it will show a background overlay to improve readability in cluttered scenes."
                ),
                value: m_background.boolValue
            );

            m_backgroundColor.colorValue = EditorGUILayout.ColorField(m_backgroundColor.colorValue);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            m_enableHotkeys.boolValue = EditorGUILayout.Toggle
            (
                new GUIContent
                (
                    text: "Enable Hotkeys",
                    tooltip: "If ticked, it will enable the hotkeys to be able to modify Graphy in runtime with custom keyboard shortcuts."
                ),
                value: m_enableHotkeys.boolValue
            );

            if (m_enableHotkeys.boolValue)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 130;
                EditorGUIUtility.fieldWidth = 35;

                EditorGUILayout.PropertyField
                (
                    m_toggleModeKeyCode,
                    new GUIContent
                    (
                        text: "Toggle Mode Key",
                        tooltip: "If ticked, it will require clicking this key and the other ones you have set up."
                    )
                );

                EditorGUIUtility.labelWidth = 30;
                EditorGUIUtility.fieldWidth = 35;

                m_toggleModeCtrl.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        text: "Ctrl",
                        tooltip: "If ticked, it will require clicking Ctrl and the other keys you have set up."
                    ),
                    value: m_toggleModeCtrl.boolValue
                );

                m_toggleModeAlt.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        text: "Alt",
                        tooltip: "If ticked, it will require clicking Alt and the other keys you have set up."
                    ),
                    value: m_toggleModeAlt.boolValue
                );

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 130;
                EditorGUIUtility.fieldWidth = 35;

                EditorGUILayout.PropertyField
                (
                    m_toggleActiveKeyCode,
                    new GUIContent
                    (
                        text: "Toggle Active Key",
                        tooltip: "If ticked, it will require clicking this key and the other ones you have set up."
                    )
                );

                EditorGUIUtility.labelWidth = 30;
                EditorGUIUtility.fieldWidth = 35;

                m_toggleActiveCtrl.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        text: "Ctrl",
                        tooltip: "If ticked, it will require clicking Ctrl and the other kesy you have set up."
                    ),
                    value: m_toggleActiveCtrl.boolValue
                );

                m_toggleActiveAlt.boolValue = EditorGUILayout.Toggle
                (
                    new GUIContent
                    (
                        text: "Alt",
                        tooltip: "If ticked, it will require clicking Alt and the other keys you have set up."
                    ),
                    value: m_toggleActiveAlt.boolValue
                );

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(15);

            EditorGUIUtility.labelWidth = 155;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField
            (
                m_graphModulePosition,
                new GUIContent
                (
                    text: "Graph modules position",
                    tooltip: "Defines in which corner the modules will be located."
                )
            );

            #endregion

            GUILayout.Space(20);

            #region Section -> FPS

            m_fpsModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_fpsModuleInspectorToggle,
                content: " [ FPS ]",
                style: GraphyEditorStyle.FoldoutStyle
            );

            GUILayout.Space(5);

            if (m_fpsModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_fpsModuleState,
                    new GUIContent
                    (
                        text: "Module state",
                        tooltip: "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.LabelField("Fps thresholds and colors:");

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_goodFpsThreshold.intValue = EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        text: "- Good",
                        tooltip: "When FPS rise above this value, this color will be used."
                    ),
                    value: m_goodFpsThreshold.intValue
                );

                m_goodFpsColor.colorValue = EditorGUILayout.ColorField(m_goodFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                if (m_goodFpsThreshold.intValue <= m_cautionFpsThreshold.intValue && m_goodFpsThreshold.intValue > 1)
                {
                    m_cautionFpsThreshold.intValue = m_goodFpsThreshold.intValue - 1;
                }
                else if (m_goodFpsThreshold.intValue <= 1)
                {
                    m_goodFpsThreshold.intValue = 2;
                }

                EditorGUILayout.BeginHorizontal();

                m_cautionFpsThreshold.intValue = EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        text: "- Caution",
                        tooltip: "When FPS falls between this and the Good value, this color will be used."
                    ),
                    value: m_cautionFpsThreshold.intValue
                );

                m_cautionFpsColor.colorValue = EditorGUILayout.ColorField(m_cautionFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                if (m_cautionFpsThreshold.intValue >= m_goodFpsThreshold.intValue)
                {
                    m_cautionFpsThreshold.intValue = m_goodFpsThreshold.intValue - 1;
                }
                else if (m_cautionFpsThreshold.intValue <= 0)
                {
                    m_cautionFpsThreshold.intValue = 1;
                }

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.IntField
                (
                    new GUIContent
                    (
                        text: "- Critical",
                        tooltip: "When FPS falls below the Caution value, this color will be used. (You can't have negative FPS, so this value is just for reference, it can't be changed)."
                    ),
                    value: 0
                );

                m_criticalFpsColor.colorValue = EditorGUILayout.ColorField(m_criticalFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;

                if (m_fpsModuleState.intValue == 0)
                {
                    m_fpsGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent
                        (
                            text: "Graph resolution",
                            tooltip: "Defines the amount of points in the graph"
                        ),
                        m_fpsGraphResolution.intValue,
                        leftValue: 20,
                        rightValue: m_graphyMode.intValue == 0 ? 300 : 128
                    );
                }

                m_fpsTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        text: "Text update rate",
                        tooltip: "Defines the amount times the text is updated in 1 second."
                    ),
                    m_fpsTextUpdateRate.intValue,
                    leftValue: 1,
                    rightValue: 60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> RAM

            m_ramModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_ramModuleInspectorToggle,
                content: " [ RAM ]",
                style: GraphyEditorStyle.FoldoutStyle
            );

            GUILayout.Space(5);

            if (m_ramModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_ramModuleState,
                    new GUIContent
                    (
                        text: "Module state",
                        tooltip: "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.LabelField("Graph colors:");

                EditorGUI.indentLevel++;

                m_allocatedRamColor.colorValue = EditorGUILayout.ColorField
                (
                    label: "- Allocated",
                    value: m_allocatedRamColor.colorValue
                );

                m_reservedRamColor.colorValue = EditorGUILayout.ColorField
                (
                    label: "- Reserved",
                    value: m_reservedRamColor.colorValue
                );

                m_monoRamColor.colorValue = EditorGUILayout.ColorField
                (
                    label: "- Mono",
                    value: m_monoRamColor.colorValue
                );

                EditorGUI.indentLevel--;

                if (m_ramModuleState.intValue == 0)
                {
                    m_ramGraphResolution.intValue = EditorGUILayout.IntSlider(
                        new GUIContent
                        (
                            text: "Graph resolution",
                            tooltip: "Defines the amount of points are in the graph"
                        ),
                        m_ramGraphResolution.intValue,
                        leftValue: 20,
                        rightValue: m_graphyMode.intValue == 0 ? 300 : 128
                    );
                }

                m_ramTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        text: "Text update rate",
                        tooltip: "Defines the amount times the text is updated in 1 second."
                    ),
                    m_ramTextUpdateRate.intValue,
                    leftValue: 1,
                    rightValue: 60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> Audio

            m_audioModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_audioModuleInspectorToggle,
                content: " [ AUDIO ]",
                style: GraphyEditorStyle.FoldoutStyle
            );

            GUILayout.Space(5);

            if (m_audioModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField
                (
                    m_audioModuleState,
                    new GUIContent
                    (
                        text: "Module state",
                        tooltip: "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"
                    )
                );

                GUILayout.Space(5);

                EditorGUILayout.PropertyField
                (
                    m_findAudioListenerInCameraIfNull,
                    new GUIContent
                    (
                        text: "Find audio listener",
                        tooltip: "Tries to find the AudioListener in the Main camera in the scene. (if AudioListener is null)"
                    )
                );

                EditorGUILayout.PropertyField
                (
                    m_audioListener,
                    new GUIContent
                    (
                        text: "Audio Listener",
                        tooltip: "Graphy will take the data from this Listener. If none are specified, it will try to get it from the Main Camera in the scene."
                    )
                );

                if (m_audioModuleState.intValue == 0)
                {
                    m_audioGraphColor.colorValue = EditorGUILayout.ColorField
                    (
                        label: "Graph color",
                        value: m_audioGraphColor.colorValue
                    );

                    m_audioGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent
                        (
                            text: "Graph resolution",
                            tooltip: "Defines the amount of points that are in the graph."
                        ),
                        m_audioGraphResolution.intValue,
                        leftValue: 20,
                        rightValue: m_graphyMode.intValue == 0 ? 300 : 128
                    );

                    // Forces the value to be a multiple of 3, this way the audio graph is painted correctly
                    if (m_audioGraphResolution.intValue % 3 != 0 && m_audioGraphResolution.intValue < 300)
                    {
                        m_audioGraphResolution.intValue += 3 - m_audioGraphResolution.intValue % 3;
                    }
                    //TODO: Figure out why a static version of the ForceMultipleOf3 isnt used.
                }

                EditorGUILayout.PropertyField
                (
                    m_FFTWindow,
                    new GUIContent
                    (
                        text: "FFT Window",
                        tooltip: "Used to reduce leakage between frequency bins/bands. Note, the more complex window type, the better the quality, but reduced speed. \n\nSimplest is rectangular. Most complex is BlackmanHarris"
                    )
                );

                m_spectrumSize.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        text: "Spectrum size",
                        tooltip: "Has to be a power of 2 between 128-8192. The higher sample rate, the less precision but also more impact on performance. Careful with mobile devices"
                    ),
                    m_spectrumSize.intValue,
                    leftValue: 128,
                    rightValue: 8192
                );

                int closestSpectrumIndex = 0;
                int minDistanceToSpectrumValue = 100000;

                for (int i = 0; i < m_spectrumSizeValues.Length; i++)
                {
                    int newDistance = Mathf.Abs
                    (
                        value: m_spectrumSize.intValue - m_spectrumSizeValues[i]
                    );

                    if (newDistance < minDistanceToSpectrumValue)
                    {
                        minDistanceToSpectrumValue = newDistance;
                        closestSpectrumIndex = i;
                    }
                }

                m_spectrumSize.intValue = m_spectrumSizeValues[closestSpectrumIndex];

                m_audioTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent
                    (
                        text: "Text update rate",
                        tooltip: "Defines the amount times the text is updated in 1 second"
                    ),
                    m_audioTextUpdateRate.intValue,
                    leftValue: 1,
                    rightValue: 60
                );
            }

            #endregion

            GUILayout.Space(20);

            #region Section -> Advanced Settings

            m_advancedModuleInspectorToggle = EditorGUILayout.Foldout
            (
                m_advancedModuleInspectorToggle,
                content: " [ ADVANCED DATA ]",
                style: GraphyEditorStyle.FoldoutStyle
            );

            GUILayout.Space(5);

            if (m_advancedModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_advancedModulePosition);

                EditorGUILayout.PropertyField
                (
                    m_advancedModuleState,
                    new GUIContent
                    (
                        text: "Module state",
                        tooltip: "FULL -> Text \nOFF -> Turned off"
                    )
                );
            }

            #endregion;

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Methods -> Private

        #endregion
    }
}