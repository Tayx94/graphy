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
    public class QTagIconComponent: QBaseComponent
    {
        private List<QTagTexture> tagTextureList;

        // CONSTRUCTOR
        public QTagIconComponent()
        {
            rect.width  = 14;
            rect.height = 14;

            QSettings.getInstance().addEventListener(QSetting.TagIconShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagIconShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagIconSize              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagIconList              , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled = QSettings.getInstance().get<bool>(QSetting.TagIconShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.TagIconShowDuringPlayMode);
            QHierarchySizeAll size = (QHierarchySizeAll)QSettings.getInstance().get<int>(QSetting.TagIconSize);
            rect.width = rect.height = (size == QHierarchySizeAll.Normal ? 15 : (size == QHierarchySizeAll.Big ? 16 : 13));        
            this.tagTextureList = QTagTexture.loadTagTextureList();
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
            string gameObjectTag = "";
            try { gameObjectTag = gameObject.tag; }
            catch {}

            QTagTexture tagTexture = tagTextureList.Find(t => t.tag == gameObjectTag);
            if (tagTexture != null && tagTexture.texture != null)
            {
                GUI.DrawTexture(rect, tagTexture.texture, ScaleMode.ScaleToFit, true);
            }
        }
    }
}

