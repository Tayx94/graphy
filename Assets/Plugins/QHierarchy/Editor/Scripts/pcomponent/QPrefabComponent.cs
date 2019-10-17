using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy.pcomponent
{
    public class QPrefabComponent: QBaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Texture2D prefabTexture;
        private bool showPrefabConnectedIcon;

        // CONSTRUCTOR
        public QPrefabComponent()
        {
            rect.width = 9;

            prefabTexture = QResources.getInstance().getTexture(QTexture.QPrefabIcon);

            QSettings.getInstance().addEventListener(QSetting.PrefabShowBreakedPrefabsOnly  , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.PrefabShow                    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalActiveColor         , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalInactiveColor       , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            showPrefabConnectedIcon = QSettings.getInstance().get<bool>(QSetting.PrefabShowBreakedPrefabsOnly);
            enabled                 = QSettings.getInstance().get<bool>(QSetting.PrefabShow);
            activeColor             = QSettings.getInstance().getColor(QSetting.AdditionalActiveColor);
            inactiveColor           = QSettings.getInstance().getColor(QSetting.AdditionalInactiveColor);
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 9)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 9;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {  
            PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
            if (prefabType == PrefabType.MissingPrefabInstance || 
                prefabType == PrefabType.DisconnectedPrefabInstance ||
                prefabType == PrefabType.DisconnectedModelPrefabInstance)
            {
                QColorUtils.setColor(inactiveColor);
                GUI.DrawTexture(rect, prefabTexture);
                QColorUtils.clearColor();
            }
            else if (!showPrefabConnectedIcon && prefabType != PrefabType.None)
            {
                QColorUtils.setColor(activeColor);
                GUI.DrawTexture(rect, prefabTexture);
                QColorUtils.clearColor();
            }
        }
    }
}

