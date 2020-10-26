using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tayx.Graphy
{
    internal static class GraphyEditorStyle
    {
        #region Variables -> Private

        private static Texture2D _managerLogoTexture = null;
        private static Texture2D _debuggerLogoTexture = null;
        private static GUISkin m_skin = null;
        private static GUIStyle m_headerStyle1 = null;
        private static GUIStyle m_headerStyle2 = null;
        private static GUIStyle m_foldoutStyle = null;
        private static string path;

        #endregion

        #region Properties -> Public

        public static Texture2D ManagerLogoTexture { get { return _managerLogoTexture; } }
        public static Texture2D DebuggerLogoTexture { get { return _debuggerLogoTexture; } }
        public static GUISkin Skin { get { return m_skin;  } }
        public static GUIStyle HeaderStyle1 { get { return m_headerStyle1; } }
        public static GUIStyle HeaderStyle2 { get { return m_headerStyle2; } }
        public static GUIStyle FoldoutStyle { get { return m_foldoutStyle; } }

        #endregion

        #region Static Constructor

        static GraphyEditorStyle()
        {
            string managerLogoGuid = AssetDatabase.FindAssets( $"Manager_Logo_{(EditorGUIUtility.isProSkin ? "White" : "Dark")}" )[0];
            string debuggerLogoGuid = AssetDatabase.FindAssets( $"Debugger_Logo_{(EditorGUIUtility.isProSkin ? "White" : "Dark")}" )[0];
            string guiSkinGuid = AssetDatabase.FindAssets( "GraphyGUISkin" )[ 0 ];

            _managerLogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                AssetDatabase.GUIDToAssetPath( managerLogoGuid )
            );

            _debuggerLogoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                AssetDatabase.GUIDToAssetPath( debuggerLogoGuid )
            );

            m_skin = AssetDatabase.LoadAssetAtPath<GUISkin>
            (
                AssetDatabase.GUIDToAssetPath( guiSkinGuid )
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

            m_foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                font = m_headerStyle2.font,
                fontStyle = m_headerStyle2.fontStyle,
                contentOffset = Vector2.down * 3f
            };

            SetGuiStyleFontColor
            (
                guiStyle: m_foldoutStyle,
                color: EditorGUIUtility.isProSkin ? Color.white : Color.black
            );
        }

        #endregion

        #region Methods -> Private

        private static void SetGuiStyleFontColor(GUIStyle guiStyle, Color color)
        {
            guiStyle.normal.textColor = color;
            guiStyle.hover.textColor = color;
            guiStyle.active.textColor = color;
            guiStyle.focused.textColor = color;
            guiStyle.onNormal.textColor = color;
            guiStyle.onHover.textColor = color;
            guiStyle.onActive.textColor = color;
            guiStyle.onFocused.textColor = color;
        }

        private static string GetPath()
        {
            ScriptableObject dummy = ScriptableObject.CreateInstance<G_SODummy>();
            MonoScript ms = MonoScript.FromScriptableObject(dummy);

            string filePath = AssetDatabase.GetAssetPath(ms);
            FileInfo fi = new FileInfo(filePath);

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

        #endregion
    }
}