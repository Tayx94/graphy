/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 20-Dec-17
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/
 
using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

namespace Tayx.Graphy
{
    [CustomEditor(typeof(GraphyManager))]
    internal class GraphyManagerEditor : Editor
    {
        #region Private Variables

        private GraphyManager m_target;

        private GUISkin m_skin;

        private GUIStyle m_headerStyle1;
        private GUIStyle m_headerStyle2;

        private Texture2D m_logoTexture;

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

        private SerializedProperty m_graphyMode;
        
        private SerializedProperty m_keepAlive;

        private SerializedProperty m_background;
        private SerializedProperty m_backgroundColor;

        
        private SerializedProperty m_toggleModeKeyCode;
        private SerializedProperty m_toggleModeCtrl;
        private SerializedProperty m_toggleModeAlt;

        private SerializedProperty m_toggleActiveKeyCode;
        private SerializedProperty m_toggleActiveCtrl;
        private SerializedProperty m_toggleActiveAlt;


        private SerializedProperty m_graphModulePosition;

        // Fps ---------------------------------------------------------------------------

        private bool m_fpsModuleInspectorToggle = true;
            
        private SerializedProperty m_fpsModuleState;

        private SerializedProperty m_timeToResetMinMaxFps;

        private SerializedProperty m_goodFpsColor;
        private SerializedProperty m_goodFpsThreshold;

        private SerializedProperty m_cautionFpsColor;
        private SerializedProperty m_cautionFpsThreshold;

        private SerializedProperty m_criticalFpsColor;

        private SerializedProperty m_fpsGraphResolution;

        private SerializedProperty m_fpsTextUpdateRate;

        // Ram ---------------------------------------------------------------------------

        private bool m_ramModuleInspectorToggle = true;

        private SerializedProperty m_ramModuleState;
            
        private SerializedProperty m_allocatedRamColor;
        private SerializedProperty m_reservedRamColor;
        private SerializedProperty m_monoRamColor;

        private SerializedProperty m_ramGraphResolution;

        private SerializedProperty m_ramTextUpdateRate;

        // Audio -------------------------------------------------------------------------

        private bool m_audioModuleInspectorToggle = true;
            
        private SerializedProperty m_findAudioListenerInCameraIfNull;

        private SerializedProperty m_audioListener;
        
        private SerializedProperty m_audioModuleState;

        private SerializedProperty m_audioGraphColor;

        private SerializedProperty m_audioGraphResolution;

        private SerializedProperty m_audioTextUpdateRate;
        
        private SerializedProperty m_FFTWindow;

        private SerializedProperty m_spectrumSize;

        // Advanced ----------------------------------------------------------------------

        private bool m_advancedModuleInspectorToggle = true;
            
        private SerializedProperty m_advancedModulePosition;

        private SerializedProperty m_advancedModuleState;


        #endregion

        #region Unity Editor Methods

        public void OnEnable()
        {
            m_target = (GraphyManager)target;

            LoadGuiStyles();

            SerializedObject serObj = serializedObject;

            m_graphyMode = serObj.FindProperty("m_graphyMode");

            m_keepAlive = serObj.FindProperty("m_keepAlive");

            m_background = serObj.FindProperty("m_background");
            m_backgroundColor = serObj.FindProperty("m_backgroundColor");

            m_toggleModeKeyCode = serObj.FindProperty("m_toggleModeKeyCode");
            m_toggleModeCtrl = serObj.FindProperty("m_toggleModeCtrl");
            m_toggleModeAlt = serObj.FindProperty("m_toggleModeAlt");

            m_toggleActiveKeyCode = serObj.FindProperty("m_toggleActiveKeyCode");
            m_toggleActiveCtrl = serObj.FindProperty("m_toggleActiveCtrl");
            m_toggleActiveAlt = serObj.FindProperty("m_toggleActiveAlt");

            m_graphModulePosition = serObj.FindProperty("m_graphModulePosition");

            // Fps ---------------------------------------------------------------------------

            m_fpsModuleState = serObj.FindProperty("m_fpsModuleState");

            m_timeToResetMinMaxFps = serObj.FindProperty("m_timeToResetMinMaxFps");

            m_goodFpsColor = serObj.FindProperty("m_goodFpsColor");
            m_goodFpsThreshold = serObj.FindProperty("m_goodFpsThreshold");

            m_cautionFpsColor = serObj.FindProperty("m_cautionFpsColor");
            m_cautionFpsThreshold = serObj.FindProperty("m_cautionFpsThreshold");

            m_criticalFpsColor = serObj.FindProperty("m_criticalFpsColor");

            m_fpsGraphResolution = serObj.FindProperty("m_fpsGraphResolution");

            m_fpsTextUpdateRate = serObj.FindProperty("m_fpsTextUpdateRate");

            // Ram ---------------------------------------------------------------------------

            m_ramModuleState = serObj.FindProperty("m_ramModuleState");
            
            m_allocatedRamColor = serObj.FindProperty("m_allocatedRamColor");
            m_reservedRamColor = serObj.FindProperty("m_reservedRamColor");
            m_monoRamColor = serObj.FindProperty("m_monoRamColor");

            m_ramGraphResolution = serObj.FindProperty("m_ramGraphResolution");

            m_ramTextUpdateRate = serObj.FindProperty("m_ramTextUpdateRate");

            // Audio -------------------------------------------------------------------------

            m_findAudioListenerInCameraIfNull = serObj.FindProperty("m_findAudioListenerInCameraIfNull");

            m_audioListener = serObj.FindProperty("m_audioListener");
            
            m_audioModuleState = serObj.FindProperty("m_audioModuleState");

            m_audioGraphColor = serObj.FindProperty("m_audioGraphColor");

            m_audioGraphResolution = serObj.FindProperty("m_audioGraphResolution");

            m_audioTextUpdateRate = serObj.FindProperty("m_audioTextUpdateRate");


            m_FFTWindow = serObj.FindProperty("m_FFTWindow");

            m_spectrumSize = serObj.FindProperty("m_spectrumSize");

            // Advanced ----------------------------------------------------------------------

            m_advancedModulePosition = serObj.FindProperty("m_advancedModulePosition");

            m_advancedModuleState = serObj.FindProperty("m_advancedModuleState");
            
        }

        public override void OnInspectorGUI()
        {
            if (m_target == null && target == null)
            {
                base.OnInspectorGUI();

                return;
            }

            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            float defaultFieldWidth = EditorGUIUtility.fieldWidth;
            
            

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);

            foldoutStyle.font = m_headerStyle2.font;
            foldoutStyle.fontStyle = m_headerStyle2.fontStyle;

            foldoutStyle.normal.textColor       = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.onNormal.textColor     = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.hover.textColor        = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.onHover.textColor      = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.focused.textColor      = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.onFocused.textColor    = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.active.textColor       = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            foldoutStyle.onActive.textColor     = EditorGUIUtility.isProSkin ? Color.white : Color.black;

            foldoutStyle.contentOffset = Vector2.down * 3f;

            GUILayout.Space(20);

            if (m_logoTexture != null)
            {
                var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
                centeredStyle.alignment = TextAnchor.UpperCenter;

                GUILayout.Label(m_logoTexture, centeredStyle);
            }
            else
            {
                EditorGUILayout.LabelField("[ GRAPHY - MANAGER ]", m_headerStyle1);
            }

            GUILayout.Space(10);
            
            EditorGUIUtility.labelWidth = 130;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField(m_graphyMode, new GUIContent("Graphy Mode", "LIGHT mode increases compatibility with older GPUs, but reduces the maximum graph resolutions to 128."));

            GUILayout.Space(10);

            m_keepAlive.boolValue = EditorGUILayout.Toggle(new GUIContent("Keep Alive", "If ticked, it will survive scene changes. Careful, if you set Graphy as a chilof another GameObject, the root GameObject will also survive scene changes. If you want to avoid that put Graphy in the root of the Scene as its own entity."), m_keepAlive.boolValue);
               
            GUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();

            m_background.boolValue = EditorGUILayout.Toggle(new GUIContent("Background", "If ticked, it will show a background overlay to improve readability in cluttered scenes."), m_background.boolValue);
            m_backgroundColor.colorValue = EditorGUILayout.ColorField(m_backgroundColor.colorValue);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 130;
            EditorGUIUtility.fieldWidth = 35;
            EditorGUILayout.PropertyField(m_toggleModeKeyCode, new GUIContent("Toggle Mode Key", "If ticked, it will require clicking this key and the other ones you have set up"));

            EditorGUIUtility.labelWidth = 30;
            EditorGUIUtility.fieldWidth = 35;
            m_toggleModeCtrl.boolValue = EditorGUILayout.Toggle(new GUIContent("Ctrl", "If ticked, it will require clicking Ctrl and the other keys you have set up"), m_toggleModeCtrl.boolValue);
            m_toggleModeAlt.boolValue = EditorGUILayout.Toggle(new GUIContent("Alt", "If ticked, it will require clicking Alt and the other keys you have set up"), m_toggleModeAlt.boolValue);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 130;
            EditorGUIUtility.fieldWidth = 35;
            EditorGUILayout.PropertyField(m_toggleActiveKeyCode, new GUIContent("Toggle Active Key", "If ticked, it will require clicking this key and the other ones you have set up"));

            EditorGUIUtility.labelWidth = 30;
            EditorGUIUtility.fieldWidth = 35;
            m_toggleActiveCtrl.boolValue = EditorGUILayout.Toggle(new GUIContent("Ctrl", "If ticked, it will require clicking Ctrl and the other keys you have set up"), m_toggleActiveCtrl.boolValue);
            m_toggleActiveAlt.boolValue = EditorGUILayout.Toggle(new GUIContent("Alt", "If ticked, it will require clicking Alt and the other keys you have set up"), m_toggleActiveAlt.boolValue);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

            EditorGUIUtility.labelWidth = 155;
            EditorGUIUtility.fieldWidth = 35;

            EditorGUILayout.PropertyField(m_graphModulePosition, new GUIContent("Graph modules position", "Defines in wich top corner the modules will be located"));
            
            GUILayout.Space(5);

            // Fps ---------------------------------------------------------------------------

            m_fpsModuleInspectorToggle = EditorGUILayout.Foldout(m_fpsModuleInspectorToggle,
                " [ FPS ]", foldoutStyle);
            
            GUILayout.Space(5);


            if (m_fpsModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_fpsModuleState, new GUIContent("Module state", "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"));
            
                GUILayout.Space(5);

                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Fps thresholds and colors:");

                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                m_goodFpsThreshold.intValue = EditorGUILayout.IntField
                (
                    new GUIContent("- Good", "When FPS rise above this value, this color will be used"), 
                    m_goodFpsThreshold.intValue
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
                    new GUIContent("- Caution", "When FPS are between this and the Good value, this color will be used"), 
                    m_cautionFpsThreshold.intValue
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
                    new GUIContent("- Critical", "When FPS are below the Caution value, this color will be used. (You can't have negative FPS, so this value is just for reference, it can't be changed)."), 
                    0
                );
                m_criticalFpsColor.colorValue = EditorGUILayout.ColorField(m_criticalFpsColor.colorValue);

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;

                if (m_fpsModuleState.intValue == 0)
                {
                    m_fpsGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent("Graph resolution", "Defines the amount of points are in the graph"),
                        m_fpsGraphResolution.intValue, 20, m_graphyMode.intValue == 0 ? 300 : 128
                    );
                }

                EditorGUIUtility.labelWidth = 200;
                EditorGUIUtility.fieldWidth = 35;

                m_timeToResetMinMaxFps.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent("Time to reset min/max values", "If the min/max value doesn't change in the specified time, they will be reset. This allows tracking the min/max fps in a shorter interval. \n\nSet to 0 if you  don't want it to reset."), 
                    m_timeToResetMinMaxFps.intValue, 0, 120
                );

                EditorGUIUtility.labelWidth = 155;
                EditorGUIUtility.fieldWidth = 35;

                m_fpsTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent("Text update rate", "Defines the amount times the text is updated in 1 second"),
                    m_fpsTextUpdateRate.intValue, 1, 60
                );

                GUILayout.Space(10);

                EditorGUI.indentLevel--;
            }
            
            // Ram ---------------------------------------------------------------------------

            m_ramModuleInspectorToggle = EditorGUILayout.Foldout(m_ramModuleInspectorToggle,
                " [ RAM ]", foldoutStyle);

            GUILayout.Space(5);

            if (m_ramModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_ramModuleState, new GUIContent("Module state", "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"));

                GUILayout.Space(5);
            
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Graph colors:");

                EditorGUI.indentLevel++;

                m_allocatedRamColor.colorValue = EditorGUILayout.ColorField("- Allocated",
                    m_allocatedRamColor.colorValue);
                m_reservedRamColor.colorValue = EditorGUILayout.ColorField("- Reserved", m_reservedRamColor.colorValue);
                m_monoRamColor.colorValue = EditorGUILayout.ColorField("- Mono", m_monoRamColor.colorValue);

                EditorGUI.indentLevel--;

                if (m_ramModuleState.intValue == 0)
                {
                    m_ramGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent("Graph resolution", "Defines the amount of points are in the graph"),
                        m_ramGraphResolution.intValue, 20, m_graphyMode.intValue == 0 ? 300 : 128
                    );
                }

                m_ramTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent("Text update rate", "Defines the amount times the text is updated in 1 second"),
                    m_ramTextUpdateRate.intValue, 1, 60
                );

                GUILayout.Space(10);

                EditorGUI.indentLevel--;
            }

            // Audio -------------------------------------------------------------------------

            m_audioModuleInspectorToggle = EditorGUILayout.Foldout(m_audioModuleInspectorToggle,
                " [ AUDIO ]", foldoutStyle);

            GUILayout.Space(5);

            if (m_audioModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_audioModuleState, new GUIContent("Module state", "FULL -> Text + Graph \nTEXT -> Just text \nOFF -> Turned off"));

                GUILayout.Space(5);
                
                EditorGUI.indentLevel++;
                
                EditorGUILayout.PropertyField
                (
                    m_findAudioListenerInCameraIfNull,
                    new GUIContent("Find audio listener", "Tries to find the AudioListener in the Main camera in the scene (if AudioListener is null)")
                );

                EditorGUILayout.PropertyField
                (
                    m_audioListener,
                    new GUIContent("Audio Listener", "Graphy will take the data from this Listener. If none is specified, it will try to get it from the Main Camera in the scene.")
                );

                if (m_audioModuleState.intValue == 0)
                {
                    m_audioGraphColor.colorValue = EditorGUILayout.ColorField("Graph color",
                        m_audioGraphColor.colorValue);

                    m_audioGraphResolution.intValue = EditorGUILayout.IntSlider
                    (
                        new GUIContent("Graph resolution", "Defines the amount of points are in the graph. \nUse a multiple of 3 for the best results"),
                        m_audioGraphResolution.intValue, 20, m_graphyMode.intValue == 0 ? 300 : 128
                    );

                    // Forces the value to be a multiple of 3, this way the audio graph is painted correctly

                    if (m_audioGraphResolution.intValue % 3 != 0 && m_audioGraphResolution.intValue < 300)
                    {
                        m_audioGraphResolution.intValue += 3 - m_audioGraphResolution.intValue % 3;
                    }
                }

                EditorGUILayout.PropertyField
                (
                    m_FFTWindow,
                    new GUIContent("FFT Window", "Used to reduce leakage between frequency bins/bands. Note, the more complex window type, the better the quality, but reduced speed. \n\nSimplest is rectangular. Most complex is BlackmanHarris")
                );

                m_spectrumSize.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent("Spectrum size", "Has to be a power of 2 between 128-8192. The higher sample rate, the less precision but also more impact on performance. Careful with mobile devices"), 
                    m_spectrumSize.intValue, 128, 8192
                );

                int closestSpectrumIndex = 0;
                int minDistanceToSpectrumValue = 100000;

                for (int i = 0; i < m_spectrumSizeValues.Length; i++)
                {
                    int newDistance = Mathf.Abs(m_spectrumSize.intValue - m_spectrumSizeValues[i]);
                    if (newDistance < minDistanceToSpectrumValue)
                    {
                        minDistanceToSpectrumValue = newDistance;
                        closestSpectrumIndex = i;
                    }
                }

                m_spectrumSize.intValue = m_spectrumSizeValues[closestSpectrumIndex];

                m_audioTextUpdateRate.intValue = EditorGUILayout.IntSlider
                (
                    new GUIContent("Text update rate", "Defines the amount times the text is updated in 1 second"),
                    m_audioTextUpdateRate.intValue, 1, 60
                );

                GUILayout.Space(10);
                
                EditorGUI.indentLevel--;
            }

            // Advanced -----------------------------------------------------------------------

            m_advancedModuleInspectorToggle = EditorGUILayout.Foldout(m_advancedModuleInspectorToggle,
                " [ ADVANCED DATA ]", foldoutStyle);

            GUILayout.Space(5);

            if (m_advancedModuleInspectorToggle)
            {
                EditorGUILayout.PropertyField(m_advancedModulePosition);

                EditorGUILayout.PropertyField(m_advancedModuleState, new GUIContent("Module state", "FULL -> Text \nOFF -> Turned off"));
            }
            
            // End ----------------------------------

            EditorGUIUtility.labelWidth = defaultLabelWidth;
            EditorGUIUtility.fieldWidth = defaultFieldWidth;

            serializedObject.ApplyModifiedProperties();
            
        }

        #endregion

        #region Private Methods

        private void LoadGuiStyles()
        {
            string path = GetMonoScriptFilePath(this);

            path = path.Split(new string[] { "Assets" }, StringSplitOptions.None)[1]
                       .Split(new string[] { "Tayx"   }, StringSplitOptions.None)[0];

            m_logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets" + path + "Tayx/Graphy - Ultimate Stats Monitor/Textures/Manager_Logo_" + (EditorGUIUtility.isProSkin ? "White.png" : "Dark.png"));

            m_skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets" + path + "Tayx/Graphy - Ultimate Stats Monitor/GUI/Graphy.guiskin");

            if (m_skin != null)
            {
                m_headerStyle1 = m_skin.GetStyle("Header1");
                m_headerStyle2 = m_skin.GetStyle("Header2");

                SetGuiStyleFontColor(m_headerStyle2, EditorGUIUtility.isProSkin ? Color.white : Color.black);
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
            MonoScript ms = MonoScript.FromScriptableObject(scriptableObject);

            string filePath = AssetDatabase.GetAssetPath(ms);

            FileInfo fi = new FileInfo(filePath);

            if (fi.Directory != null)
            {
                filePath = fi.Directory.ToString();

                filePath = filePath.Replace('\\', '/');

                return filePath;
            }
            else
            {
                return null;
            }
            
        }

        #endregion
    }
}