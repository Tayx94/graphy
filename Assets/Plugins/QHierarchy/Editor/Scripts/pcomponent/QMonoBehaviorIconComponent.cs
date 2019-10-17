using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy.pcomponent
{
    public class QMonoBehaviorIconComponent: QBaseComponent
    {
        // CONST
        private const float TREE_STEP_WIDTH  = 14.0f;
        private const float TREE_STEP_HEIGHT = 16.0f;

        // PRIVATE
        private Texture2D monoBehaviourIconTexture;   
        private Texture2D monoBehaviourIconObjectTexture; 
        private bool ignoreUnityMonobehaviour;
        private Color iconColor;
        private bool showTreeMap;

        // CONSTRUCTOR 
        public QMonoBehaviorIconComponent()
        {
            rect.width  = 14;
            rect.height = 16;
            
            monoBehaviourIconTexture = QResources.getInstance().getTexture(QTexture.QMonoBehaviourIcon);
            monoBehaviourIconObjectTexture  = QResources.getInstance().getTexture(QTexture.QTreeMapObject);

            QSettings.getInstance().addEventListener(QSetting.MonoBehaviourIconIgnoreUnityMonobehaviour , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.MonoBehaviourIconShow                     , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.MonoBehaviourIconShowDuringPlayMode       , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.MonoBehaviourIconColor                    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TreeMapShow                               , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            ignoreUnityMonobehaviour    = QSettings.getInstance().get<bool>(QSetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
            enabled                     = QSettings.getInstance().get<bool>(QSetting.MonoBehaviourIconShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.MonoBehaviourIconShowDuringPlayMode);
            iconColor                   = QSettings.getInstance().getColor(QSetting.MonoBehaviourIconColor);
            showTreeMap                 = QSettings.getInstance().get<bool>(QSetting.TreeMapShow);
            EditorApplication.RepaintHierarchyWindow();  
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            bool foundCustomComponent = false;   
            if (ignoreUnityMonobehaviour)
            {
                Component[] components = gameObject.GetComponents<MonoBehaviour>();                
                for (int i = components.Length - 1; i >= 0; i--)
                {
                    if (components[i] != null && !components[i].GetType().FullName.Contains("UnityEngine")) 
                    {
                        foundCustomComponent = true;
                        break;
                    }
                }                
            }
            else
            {
                foundCustomComponent = gameObject.GetComponent<MonoBehaviour>() != null;
            }

            if (foundCustomComponent)
            {
                int ident = Mathf.FloorToInt(selectionRect.x / TREE_STEP_WIDTH) - 1;

                #if UNITY_5_6_OR_NEWER
                    ident += 0;
                #elif UNITY_5_3_OR_NEWER
                    ident += 1;
                #endif

                rect.x = ident * TREE_STEP_WIDTH;
                rect.y = selectionRect.y; 
                rect.width = 16;

                QColorUtils.setColor(iconColor);
                GUI.DrawTexture(rect, monoBehaviourIconTexture);
                QColorUtils.clearColor();

                if (!showTreeMap && gameObject.transform.childCount == 0)
                {
                    rect.width = 14;
                    GUI.DrawTexture(rect, monoBehaviourIconObjectTexture);
                }
            }
        }
    }
}

