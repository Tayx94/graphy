using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy.pcomponent
{
    public class QColorComponent: QBaseComponent
    {
        // PRIVATE
        private Color inactiveColor;
        private Texture2D colorTexture;
        private Rect colorRect = new Rect();

        // CONSTRUCTOR
        public QColorComponent()
        {
            colorTexture = QResources.getInstance().getTexture(QTexture.QColorButton);

            rect.width = 8;
            rect.height = 16;

            QSettings.getInstance().addEventListener(QSetting.ColorShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ColorShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalInactiveColor, settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = QSettings.getInstance().get<bool>(QSetting.ColorShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.ColorShowDuringPlayMode);
            inactiveColor               = QSettings.getInstance().getColor(QSetting.AdditionalInactiveColor);
        }

        // LAYOUT
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 8)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 8;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }

        // DRAW
        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            if (objectList != null)
            {
                Color newColor;
                if (objectList.gameObjectColor.TryGetValue(gameObject, out newColor))
                {
                    colorRect.Set(rect.x + 1, rect.y + 1, 5, rect.height - 1);
                    EditorGUI.DrawRect(colorRect, newColor);
                    return;
                }
            }

            QColorUtils.setColor(inactiveColor);
            GUI.DrawTexture(rect, colorTexture, ScaleMode.StretchToFill, true, 1);
            QColorUtils.clearColor();
        }

        // EVENTS
        public override void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    try {
                        PopupWindow.Show(rect, new QColorPickerWindow(Selection.Contains(gameObject) ? Selection.gameObjects : new GameObject[] { gameObject }, colorSelectedHandler, colorRemovedHandler));
                    } 
                    catch {}
                }
                currentEvent.Use();
            }
        }

        // PRIVATE
        private void colorSelectedHandler(GameObject[] gameObjects, Color color)
        {
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                QObjectList objectList = QObjectListManager.getInstance().getObjectList(gameObjects[i], true);
                Undo.RecordObject(objectList, "Color Changed");
                if (objectList.gameObjectColor.ContainsKey(gameObject))
                {
                    objectList.gameObjectColor[gameObject] = color;
                }
                else
                {
                    objectList.gameObjectColor.Add(gameObject, color);
                }                
            }
            EditorApplication.RepaintHierarchyWindow();
        }

        private void colorRemovedHandler(GameObject[] gameObjects)
        {
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                QObjectList objectList = QObjectListManager.getInstance().getObjectList(gameObjects[i], true);
                if (objectList.gameObjectColor.ContainsKey(gameObject))                
                {
                    Undo.RecordObject(objectList, "Color Changed");
                    objectList.gameObjectColor.Remove(gameObject);                          
                }
            }
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}

