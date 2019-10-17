using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy.pcomponent
{
    public class QSeparatorComponent: QBaseComponent
    {
        // PRIVATE
        private Color separatorColor;
        private Color evenShadingColor;
        private Color oddShadingColor;
        private bool showRowShading;

        // CONSTRUCTOR
        public QSeparatorComponent ()
        {
            showComponentDuringPlayMode = true;

            QSettings.getInstance().addEventListener(QSetting.SeparatorShowRowShading   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.SeparatorShow             , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.SeparatorColor                , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.SeparatorEvenRowShadingColor  , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.SeparatorOddRowShadingColor , settingsChanged);

            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            showRowShading   = QSettings.getInstance().get<bool>(QSetting.SeparatorShowRowShading);
            enabled          = QSettings.getInstance().get<bool>(QSetting.SeparatorShow);
            evenShadingColor = QSettings.getInstance().getColor(QSetting.SeparatorEvenRowShadingColor);
            oddShadingColor  = QSettings.getInstance().getColor(QSetting.SeparatorOddRowShadingColor);
            separatorColor   = QSettings.getInstance().getColor(QSetting.SeparatorColor);
        }

        // DRAW
        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            rect.y = selectionRect.y;
            rect.width = selectionRect.width + selectionRect.x;
            rect.height = 1;
            rect.x = 0;

            EditorGUI.DrawRect(rect, separatorColor);

            if (showRowShading)
            {
                selectionRect.width += selectionRect.x;
                selectionRect.x = 0;
                selectionRect.height -=1;
                selectionRect.y += 1;
                EditorGUI.DrawRect(selectionRect, ((Mathf.FloorToInt(((selectionRect.y - 4) / 16) % 2) == 0)) ? evenShadingColor : oddShadingColor);
            }
        }
    }
}

