using UnityEngine;
using UnityEditor;
using System;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using System.Collections.Generic;
using System.Collections;

namespace qtools.qhierarchy.pcomponent
{
    public class QTreeMapComponent: QBaseComponent
    {
        // CONST
        private const float TREE_STEP_WIDTH  = 14.0f;
        
        // PRIVATE
        private Texture2D treeMapLevelTexture;       
        private Texture2D treeMapLevel4Texture;       
        private Texture2D treeMapCurrentTexture;   
        private Texture2D treeMapLastTexture;
        private Texture2D treeMapObjectTexture;    
        private bool enhanced;
        private bool transparentBackground;
        private Color backgroundColor;
        private Color treeMapColor;
        
        // CONSTRUCTOR
        public QTreeMapComponent()
        { 
            backgroundColor = QResources.getInstance().getColor(QColor.Background);

            treeMapLevelTexture   = QResources.getInstance().getTexture(QTexture.QTreeMapLevel);
            treeMapLevel4Texture  = QResources.getInstance().getTexture(QTexture.QTreeMapLevel4);
            treeMapCurrentTexture = QResources.getInstance().getTexture(QTexture.QTreeMapCurrent);
            treeMapObjectTexture  = QResources.getInstance().getTexture(QTexture.QTreeMapObject);
            treeMapLastTexture    = QResources.getInstance().getTexture(QTexture.QTreeMapLast);
            
            rect.width  = 14;
            rect.height = 16;
            
            showComponentDuringPlayMode = true;
            
            QSettings.getInstance().addEventListener(QSetting.TreeMapShow           , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TreeMapColor          , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TreeMapEnhanced       , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TreeMapTransparentBackground, settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled             = QSettings.getInstance().get<bool>(QSetting.TreeMapShow);
            treeMapColor        = QSettings.getInstance().getColor(QSetting.TreeMapColor);
            enhanced            = QSettings.getInstance().get<bool>(QSetting.TreeMapEnhanced);
            transparentBackground = QSettings.getInstance().get<bool>(QSetting.TreeMapTransparentBackground);
        }
        
        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            rect.y = selectionRect.y;
            
            if (!transparentBackground) 
            {
                rect.x = 0;
                
                rect.width = selectionRect.x - 14;
                EditorGUI.DrawRect(rect, backgroundColor);
                rect.width = 14;
            }

            return QLayoutStatus.Success;
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            int childCount = gameObject.transform.childCount;
            int level = Mathf.RoundToInt(selectionRect.x / 14.0f);

            if (enhanced)
            {
                Transform gameObjectTransform = gameObject.transform;
                Transform parentTransform = null;

                for (int i = 0, j = level - 1; j >= 0; i++, j--)
                {
                    rect.x = 14 * j;
                    if (i == 0)
                    {
                        if (childCount == 0) GUI.DrawTexture(rect, treeMapObjectTexture);                    
                        gameObjectTransform = gameObject.transform;
                    }
                    else if (i == 1)
                    {
                        QColorUtils.setColor(treeMapColor);
                        if (parentTransform == null)                        
                            GUI.DrawTexture(rect, treeMapCurrentTexture);                        
                        else if (gameObjectTransform.GetSiblingIndex() == parentTransform.childCount - 1)
                            GUI.DrawTexture(rect, treeMapLastTexture);
                        else
                            GUI.DrawTexture(rect, treeMapCurrentTexture);
                        gameObjectTransform = parentTransform;
                    }
                    else
                    {
                        if (parentTransform == null)                        
                            GUI.DrawTexture(rect, treeMapLevelTexture);
                        else if (gameObjectTransform.GetSiblingIndex() != parentTransform.childCount - 1)                        
                            GUI.DrawTexture(rect, treeMapLevelTexture);

                        gameObjectTransform = parentTransform;                       
                    }
                    if (gameObjectTransform != null) parentTransform = gameObjectTransform.parent;
                }
                QColorUtils.clearColor();
            }
            else
            {
                for (int i = 0, j = level - 1; j >= 0; i++, j--)
                {
                    rect.x = 14 * j;
                    if (i == 0)
                    {
                        if (childCount > 0) continue;                    
                        else GUI.DrawTexture(rect, treeMapObjectTexture);
                    }
                    else if (i == 1)
                    {
                        QColorUtils.setColor(treeMapColor);
                        GUI.DrawTexture(rect, treeMapCurrentTexture);
                    }
                    else
                    {
                        rect.width = 14 * 4;
                        rect.x -= 14 * 3;
                        j -= 3;
                        GUI.DrawTexture(rect, treeMapLevel4Texture);
                        rect.width = 14;
                    }
                }
                QColorUtils.clearColor();
            }
        }
    }
}

