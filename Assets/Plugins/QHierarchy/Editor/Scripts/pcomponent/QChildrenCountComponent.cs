using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pdata;

namespace qtools.qhierarchy.pcomponent
{
    public class QChildrenCountComponent: QBaseComponent 
    {
        // PRIVATE
        private GUIStyle labelStyle;

        // CONSTRUCTOR
        public QChildrenCountComponent ()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 9;
            labelStyle.clipping = TextClipping.Clip;  
            labelStyle.alignment = TextAnchor.MiddleRight;

            rect.width = 22;
            rect.height = 16;

            QSettings.getInstance().addEventListener(QSetting.ChildrenCountShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountLabelSize         , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ChildrenCountLabelColor        , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled = QSettings.getInstance().get<bool>(QSetting.ChildrenCountShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.ChildrenCountShowDuringPlayMode);
            QHierarchySize labelSize = (QHierarchySize)QSettings.getInstance().get<int>(QSetting.ChildrenCountLabelSize);
            labelStyle.normal.textColor = QSettings.getInstance().getColor(QSetting.ChildrenCountLabelColor);
            labelStyle.fontSize = labelSize == QHierarchySize.Normal ? 8 : 9;
            rect.width = labelSize == QHierarchySize.Normal ? 17 : 22;
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {  
            int childrenCount = gameObject.transform.childCount;
            if (childrenCount > 0) GUI.Label(rect, childrenCount.ToString(), labelStyle);
        }
    }
}

