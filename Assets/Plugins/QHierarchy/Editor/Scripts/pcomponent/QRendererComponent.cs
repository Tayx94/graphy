using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy.pcomponent
{
    public class QRendererComponent: QBaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Color specialColor;
        private Texture2D rendererButtonTexture;
        private int targetRendererMode = -1; 

        // CONSTRUCTOR
        public QRendererComponent()
        {
            rect.width = 12;

            rendererButtonTexture = QResources.getInstance().getTexture(QTexture.QRendererButton);

            QSettings.getInstance().addEventListener(QSetting.RendererShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.RendererShowDuringPlayMode, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalActiveColor     , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalInactiveColor   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalSpecialColor    , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = QSettings.getInstance().get<bool>(QSetting.RendererShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.RendererShowDuringPlayMode);
            activeColor                 = QSettings.getInstance().getColor(QSetting.AdditionalActiveColor);
            inactiveColor               = QSettings.getInstance().getColor(QSetting.AdditionalInactiveColor);
            specialColor                = QSettings.getInstance().getColor(QSetting.AdditionalSpecialColor);
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 12)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 12;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }

        public override void disabledHandler(GameObject gameObject, QObjectList objectList)
        {
            if (objectList != null && objectList.wireframeHiddenObjects.Contains(gameObject))
            {      
                objectList.wireframeHiddenObjects.Remove(gameObject);
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null) setSelectedRenderState(renderer, false);
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                bool wireframeHiddenObjectsContains = isWireframeHidden(gameObject, objectList);
                if (wireframeHiddenObjectsContains)
                {
                    QColorUtils.setColor(specialColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    QColorUtils.clearColor();
                }
                else if (renderer.enabled)
                {
                    QColorUtils.setColor(activeColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    QColorUtils.clearColor();
                }
                else
                {
                    QColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    QColorUtils.clearColor();
                }
            }
        }

        public override void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    bool wireframeHiddenObjectsContains = isWireframeHidden(gameObject, objectList);
                    bool isEnabled = renderer.enabled;
                    
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        targetRendererMode = ((!isEnabled) == true ? 1 : 0);
                    }
                    else if (currentEvent.type == EventType.MouseDrag && targetRendererMode != -1)
                    {
                        if (targetRendererMode == (isEnabled == true ? 1 : 0)) return;
                    } 
                    else
                    {
                        targetRendererMode = -1;
                        return;
                    }

                    Undo.RecordObject(renderer, "renderer visibility change");                    
                    
                    if (currentEvent.control || currentEvent.command)
                    {
                        if (!wireframeHiddenObjectsContains)
                        {
                            setSelectedRenderState(renderer, true);
                            SceneView.RepaintAll();
                            setWireframeMode(gameObject, objectList, true);
                        }
                    }
                    else
                    {
                        if (wireframeHiddenObjectsContains)
                        {
                            setSelectedRenderState(renderer, false);
                            SceneView.RepaintAll();
                            setWireframeMode(gameObject, objectList, false);
                        }
                        else
                        {
                            Undo.RecordObject(renderer, isEnabled ? "Disable Component" : "Enable Component");
                            renderer.enabled = !isEnabled;
                        }
                    }
                    
                    EditorUtility.SetDirty(gameObject);
                }
                currentEvent.Use();
            }
        }

        // PRIVATE
        public bool isWireframeHidden(GameObject gameObject, QObjectList objectList)
        {
            return objectList == null ? false : objectList.wireframeHiddenObjects.Contains(gameObject);
        }
        
        public void setWireframeMode(GameObject gameObject, QObjectList objectList, bool targetWireframe)
        {
            if (objectList == null && targetWireframe) objectList = QObjectListManager.getInstance().getObjectList(gameObject, true);
            if (objectList != null)
            {
                Undo.RecordObject(objectList, "Renderer Visibility Change");
                if (targetWireframe) objectList.wireframeHiddenObjects.Add(gameObject);
                else objectList.wireframeHiddenObjects.Remove(gameObject);
                EditorUtility.SetDirty(objectList);
            }
        }

        static public void setSelectedRenderState(Renderer renderer, bool visible)
        {
            #if UNITY_5_5_OR_NEWER
            EditorUtility.SetSelectedRenderState(renderer, visible ? EditorSelectedRenderState.Wireframe : EditorSelectedRenderState.Hidden);
            #else
            EditorUtility.SetSelectedWireframeHidden(renderer, visible);
            #endif
        }
    }
}

