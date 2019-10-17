using System;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pdata;
using System.Reflection;

namespace qtools.qhierarchy.pcomponent
{
    public class QGameObjectIconComponent: QBaseComponent
    {
        // PRIVATE
        private MethodInfo getIconMethodInfo;
        private object[] getIconMethodParams;

        // CONSTRUCTOR
        public QGameObjectIconComponent ()
        {
            rect.width = 14;
            rect.height = 14;

            getIconMethodInfo   = typeof(EditorGUIUtility).GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static );
            getIconMethodParams = new object[1];

            QSettings.getInstance().addEventListener(QSetting.GameObjectIconShow                 , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.GameObjectIconShowDuringPlayMode   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.GameObjectIconSize                          , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled = QSettings.getInstance().get<bool>(QSetting.GameObjectIconShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.GameObjectIconShowDuringPlayMode);
            QHierarchySizeAll size = (QHierarchySizeAll)QSettings.getInstance().get<int>(QSetting.GameObjectIconSize);
            rect.width = rect.height = (size == QHierarchySizeAll.Normal ? 15 : (size == QHierarchySizeAll.Big ? 16 : 13));     
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width + 2)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y - (rect.height - 16) / 2;
                return QLayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {                      
            getIconMethodParams[0] = gameObject;
            Texture2D icon = (Texture2D)getIconMethodInfo.Invoke(null, getIconMethodParams );    
            if (icon != null) 
                GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit, true);
        }
                
        public override void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();

                Type iconSelectorType = Assembly.Load("UnityEditor").GetType("UnityEditor.IconSelector");
                MethodInfo showIconSelectorMethodInfo = iconSelectorType.GetMethod("ShowAtPosition", BindingFlags.Static | BindingFlags.NonPublic);
                showIconSelectorMethodInfo.Invoke(null, new object[] { gameObject, rect, true });
            }
        }
    }
}

