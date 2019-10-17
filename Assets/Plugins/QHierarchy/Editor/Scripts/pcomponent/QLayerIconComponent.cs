using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pdata;

namespace qtools.qhierarchy.pcomponent
{
    public class QLayerIconComponent: QBaseComponent
    {
        private List<QLayerTexture> layerTextureList;

        // CONSTRUCTOR
        public QLayerIconComponent()
        {
            rect.width  = 14;
            rect.height = 14;

            QSettings.getInstance().addEventListener(QSetting.LayerIconShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LayerIconShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LayerIconSize              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LayerIconList              , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = QSettings.getInstance().get<bool>(QSetting.LayerIconShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.LayerIconShowDuringPlayMode);
            QHierarchySizeAll size      = (QHierarchySizeAll)QSettings.getInstance().get<int>(QSetting.LayerIconSize);
            rect.width = rect.height    = (size == QHierarchySizeAll.Normal ? 15 : (size == QHierarchySizeAll.Big ? 16 : 13));        
            this.layerTextureList = QLayerTexture.loadLayerTextureList();
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
                rect.y = curRect.y - (rect.height - 16) / 2;
                return QLayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {                       
            string gameObjectLayerName = LayerMask.LayerToName(gameObject.layer);

            QLayerTexture layerTexture = layerTextureList.Find(t => t.layer == gameObjectLayerName);
            if (layerTexture != null && layerTexture.texture != null)
            {
                GUI.DrawTexture(rect, layerTexture.texture, ScaleMode.ScaleToFit, true);
            }
        }
    }
}

