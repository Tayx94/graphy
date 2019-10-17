using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using qtools.qhierarchy.pcomponent;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;
using System.Reflection;

namespace qtools.qhierarchy.phierarchy
{
    public class QHierarchy
    {
        // PRIVATE
        private HashSet<int> errorHandled = new HashSet<int>();      
        private Dictionary<int, QBaseComponent> componentDictionary;          
        private List<QBaseComponent> preComponents;
        private List<QBaseComponent> orderedComponents;
        private bool hideIconsIfThereIsNoFreeSpace;
        private int indentation;
        private Texture2D trimIcon;
        private Color backgroundColor;
        private Color inactiveColor;

        // CONSTRUCTOR
        public QHierarchy ()
        {           
            componentDictionary = new Dictionary<int, QBaseComponent>();
            componentDictionary.Add((int)QHierarchyComponentEnum.LockComponent             , new QLockComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.VisibilityComponent       , new QVisibilityComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.StaticComponent           , new QStaticComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.RendererComponent         , new QRendererComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.TagAndLayerComponent      , new QTagLayerComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.GameObjectIconComponent   , new QGameObjectIconComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.ErrorComponent            , new QErrorComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.TagIconComponent          , new QTagIconComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.LayerIconComponent        , new QLayerIconComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.ColorComponent            , new QColorComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.ComponentsComponent       , new QComponentsComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.ChildrenCountComponent    , new QChildrenCountComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.PrefabComponent           , new QPrefabComponent());
            componentDictionary.Add((int)QHierarchyComponentEnum.VerticesAndTrianglesCount , new QVerticesAndTrianglesCountComponent());

            preComponents = new List<QBaseComponent>();
            preComponents.Add(new QMonoBehaviorIconComponent());
            preComponents.Add(new QTreeMapComponent());
            preComponents.Add(new QSeparatorComponent());

            orderedComponents = new List<QBaseComponent>();

            trimIcon = QResources.getInstance().getTexture(QTexture.QTrimIcon);

            QSettings.getInstance().addEventListener(QSetting.AdditionalIdentation             , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ComponentsOrder                  , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalHideIconsIfNotFit      , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalBackgroundColor        , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalInactiveColor          , settingsChanged);
            settingsChanged();
        }
         
        // PRIVATE
        private void settingsChanged()
        {
            string componentOrder = QSettings.getInstance().get<string>(QSetting.ComponentsOrder);
            string[] componentIds = componentOrder.Split(';');
            if (componentIds.Length != QSettings.DEFAULT_ORDER_COUNT) 
            {
                QSettings.getInstance().set(QSetting.ComponentsOrder, QSettings.DEFAULT_ORDER, false);
                componentIds = QSettings.DEFAULT_ORDER.Split(';');
            }

            orderedComponents.Clear(); 
            for (int i = 0; i < componentIds.Length; i++)                
                orderedComponents.Add(componentDictionary[int.Parse(componentIds[i])]);
            orderedComponents.Add(componentDictionary[(int)QHierarchyComponentEnum.ComponentsComponent]);

            indentation                     = QSettings.getInstance().get<int>(QSetting.AdditionalIdentation);
            hideIconsIfThereIsNoFreeSpace   = QSettings.getInstance().get<bool>(QSetting.AdditionalHideIconsIfNotFit);
            backgroundColor                 = QSettings.getInstance().getColor(QSetting.AdditionalBackgroundColor);
            inactiveColor                   = QSettings.getInstance().getColor(QSetting.AdditionalInactiveColor);
        } 

        public void hierarchyWindowItemOnGUIHandler(int instanceId, Rect selectionRect)
        {
            try
            {
                QColorUtils.setDefaultColor(GUI.color);

                GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceId);
                if (gameObject == null) return;

                Rect curRect = new Rect(selectionRect);
                curRect.width = 16;
                curRect.x += selectionRect.width - indentation;

                float gameObjectNameWidth = hideIconsIfThereIsNoFreeSpace ? GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x : 0;

                QObjectList objectList = QObjectListManager.getInstance().getObjectList(gameObject, false);

                drawComponents(orderedComponents, selectionRect, ref curRect, gameObject, objectList, true, hideIconsIfThereIsNoFreeSpace ? selectionRect.x + gameObjectNameWidth + 7 : 0);    

                errorHandled.Remove(instanceId);
            }
            catch (Exception exception)
            {
                if (errorHandled.Add(instanceId))
                {
                    Debug.LogError(exception.ToString());
                }
            }
        }

        private void drawComponents(List<QBaseComponent> components, Rect selectionRect, ref Rect rect, GameObject gameObject, QObjectList objectList, bool drawBackground = false, float minX = 50)
        {
            if (Event.current.type == EventType.Repaint)
            {
                int toComponent = components.Count;
                QLayoutStatus layoutStatus = QLayoutStatus.Success;
                for (int i = 0, n = toComponent; i < n; i++)
                {
                    QBaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        layoutStatus = component.layout(gameObject, objectList, selectionRect, ref rect, rect.x - minX);
                        if (layoutStatus != QLayoutStatus.Success)
                        {
                            toComponent = layoutStatus == QLayoutStatus.Failed ? i : i + 1;
                            rect.x -= 7;

                            break;
                        }
                    }
                    else
                    {
                        component.disabledHandler(gameObject, objectList);
                    }
                } 

                if (drawBackground)
                {
                    if (backgroundColor.a != 0)
                    {
                        rect.width = selectionRect.x + selectionRect.width - rect.x;
                        EditorGUI.DrawRect(rect, backgroundColor);
                    }
                    drawComponents(preComponents    , selectionRect, ref rect, gameObject, objectList);
                }

                for (int i = 0, n = toComponent; i < n; i++)
                {
                    QBaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        component.draw(gameObject, objectList, selectionRect);
                    }
                }

                if (layoutStatus != QLayoutStatus.Success)
                {
                    rect.width = 7;
                    QColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, trimIcon);
                    QColorUtils.clearColor();
                }
            }
            else if (Event.current.isMouse)
            {
                for (int i = 0, n = components.Count; i < n; i++)
                {
                    QBaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        if (component.layout(gameObject, objectList, selectionRect, ref rect, rect.x - minX) != QLayoutStatus.Failed)
                        {
                            component.eventHandler(gameObject, objectList, Event.current);
                        }
                    }
                }
            }
        }
    }
}

