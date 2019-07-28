using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tayx.Graphy
{
    internal static class GraphyEditorStyle
    {
        public static Texture2D m_logoTexture = null;
        public static GUISkin m_skin = null;
        public static GUIStyle m_headerStyle1 = null;
        public static GUIStyle m_headerStyle2 = null;
        public static GUIStyle foldoutStyle = null;

        private static string path;

        static GraphyEditorStyle()
        {
            path = GetPath();
            path = path.Split(new string[] { "Assets" },StringSplitOptions.None)[1]
                       .Split(new string[] { "Graphy" },StringSplitOptions.None)[0];

            m_logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>
            (
                "Assets" + 
                path +
                "Graphy - Ultimate Stats Monitor/Textures/Manager_Logo_" +
                (EditorGUIUtility.isProSkin ? "White.png" : "Dark.png")
            );

            m_skin = AssetDatabase.LoadAssetAtPath<GUISkin>
            (
                "Assets" + 
                path +
                "Graphy - Ultimate Stats Monitor/GUI/Graphy.guiskin"
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

            foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                font = m_headerStyle2.font,
                fontStyle = m_headerStyle2.fontStyle,
                contentOffset = Vector2.down * 3f
            };

            SetGuiStyleFontColor
            (
                guiStyle: foldoutStyle,
                color: EditorGUIUtility.isProSkin ? Color.white : Color.black
            );
        }

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
            ScriptableObject dummy = ScriptableObject.CreateInstance<SODummy>();
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
    }
}