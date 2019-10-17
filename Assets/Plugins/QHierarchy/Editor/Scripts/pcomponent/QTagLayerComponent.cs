using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;
using System.Reflection;

namespace qtools.qhierarchy.pcomponent
{
    public class QTagLayerComponent: QBaseComponent
    {
        // PRIVATE
        private GUIStyle labelStyle;
        private QHierarchyTagAndLayerShowType showType;
        private Color layerColor;
        private Color tagColor;       
        private bool showAlways;
        private bool sizeIsPixel;
        private int pixelSize;
        private float percentSize;
        private GameObject[] gameObjects;
        private float labelAlpha;
        private QHierarchyTagAndLayerLabelSize labelSize;
        private Rect tagRect = new Rect();
        private Rect layerRect = new Rect();
        private bool needDrawTag;
        private bool needDrawLayer;
        private int layer;
        private string tag;

        // CONSTRUCTOR
        public QTagLayerComponent()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 8;
            labelStyle.clipping = TextClipping.Clip;  
            labelStyle.alignment = TextAnchor.MiddleLeft;

            QSettings.getInstance().addEventListener(QSetting.TagAndLayerSizeShowType       , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerType               , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerSizeValueType      , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerSizeValuePixel     , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerSizeValuePercent   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerLabelSize          , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerShow               , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerShowDuringPlayMode , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerTagLabelColor      , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerLayerLabelColor    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerAligment           , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.TagAndLayerLabelAlpha         , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            showAlways  = QSettings.getInstance().get<int>(QSetting.TagAndLayerType) == (int)QHierarchyTagAndLayerType.Always;
            showType    = (QHierarchyTagAndLayerShowType)QSettings.getInstance().get<int>(QSetting.TagAndLayerSizeShowType);
            sizeIsPixel = QSettings.getInstance().get<int>(QSetting.TagAndLayerSizeValueType) == (int)QHierarchyTagAndLayerSizeType.Pixel;
            pixelSize   = QSettings.getInstance().get<int>(QSetting.TagAndLayerSizeValuePixel);
            percentSize = QSettings.getInstance().get<float>(QSetting.TagAndLayerSizeValuePercent);
            labelSize   = (QHierarchyTagAndLayerLabelSize)QSettings.getInstance().get<int>(QSetting.TagAndLayerLabelSize);
            enabled     = QSettings.getInstance().get<bool>(QSetting.TagAndLayerShow);
            tagColor    = QSettings.getInstance().getColor(QSetting.TagAndLayerTagLabelColor);
            layerColor  = QSettings.getInstance().getColor(QSetting.TagAndLayerLayerLabelColor);
            labelAlpha  = QSettings.getInstance().get<float>(QSetting.TagAndLayerLabelAlpha);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.TagAndLayerShowDuringPlayMode);

            QHierarchyTagAndLayerAligment aligment = (QHierarchyTagAndLayerAligment)QSettings.getInstance().get<int>(QSetting.TagAndLayerAligment);
            switch (aligment)
            {
                case QHierarchyTagAndLayerAligment.Left  : labelStyle.alignment = TextAnchor.MiddleLeft;   break;
                case QHierarchyTagAndLayerAligment.Center: labelStyle.alignment = TextAnchor.MiddleCenter; break;
                case QHierarchyTagAndLayerAligment.Right : labelStyle.alignment = TextAnchor.MiddleRight;  break;
            }
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            float textWidth = sizeIsPixel ? pixelSize : percentSize * rect.x;
            rect.width = textWidth + 4;

            if (maxWidth < rect.width)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;

                layer  = gameObject.layer; 
                tag = getTagName(gameObject);             
                
                needDrawTag   = (showType != QHierarchyTagAndLayerShowType.Layer) && ((showAlways || tag   != "Untagged"));
                needDrawLayer = (showType != QHierarchyTagAndLayerShowType.Tag  ) && ((showAlways || layer != 0         ));
                
                if (labelSize == QHierarchyTagAndLayerLabelSize.Big || (labelSize == QHierarchyTagAndLayerLabelSize.BigIfSpecifiedOnlyTagOrLayer && needDrawTag != needDrawLayer)) 
                    labelStyle.fontSize = 9;
                else 
                    labelStyle.fontSize = 8;
                
                if (needDrawTag) tagRect.Set(rect.x, rect.y - (needDrawLayer ? 4 : 0), rect.width, 16);                                                   
                if (needDrawLayer) layerRect.Set(rect.x, rect.y + (needDrawTag ? 4 : 0), rect.width, 16);                    

                return QLayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            if (needDrawTag ) 
            {
                tagColor.a = (tag == "Untagged" ? labelAlpha : 1.0f);
                labelStyle.normal.textColor = tagColor;
                EditorGUI.LabelField(tagRect, tag, labelStyle);
            }

            if (needDrawLayer) 
            {
                layerColor.a = (layer == 0 ? labelAlpha : 1.0f);
                labelStyle.normal.textColor = layerColor;
                EditorGUI.LabelField(layerRect, getLayerName(layer), labelStyle);
            }
        }

        public override void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {                       
            if (Event.current.isMouse && currentEvent.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (needDrawTag && needDrawLayer)
                {
                    tagRect.height = 8;
                    layerRect.height = 8;
                    tagRect.y += 4;
                    layerRect.y += 4;
                }

                if (needDrawTag && tagRect.Contains(Event.current.mousePosition))
                {
                    gameObjects = Selection.Contains(gameObject) ? Selection.gameObjects : new GameObject[] { gameObject };
                    showTagsContextMenu(tag);
                    Event.current.Use();
                }
                else if (needDrawLayer && layerRect.Contains(Event.current.mousePosition))
                {
                    gameObjects = Selection.Contains(gameObject) ? Selection.gameObjects : new GameObject[] { gameObject };
                    showLayersContextMenu(LayerMask.LayerToName(layer));
                    Event.current.Use();
                }
            }
        }

        private string getTagName(GameObject gameObject)
        {
            string tag = "Undefined";
            try { tag = gameObject.tag; }
            catch {}
            return tag;
        }

        public string getLayerName(int layer)
        {
            string layerName = LayerMask.LayerToName(layer);
            if (layerName.Equals("")) layerName = "Undefined";
            return layerName;
        }

        // PRIVATE
        private void showTagsContextMenu(string tag)
        {
            List<string> tags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Untagged"  ), false, tagChangedHandler, "Untagged");
            
            for (int i = 0, n = tags.Count; i < n; i++)
            {
                string curTag = tags[i];
                menu.AddItem(new GUIContent(curTag), tag == curTag, tagChangedHandler, curTag);
            }
            
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Tag..."  ), false, addTagOrLayerHandler, "Tags");
            menu.ShowAsContext();
        }

        private void showLayersContextMenu(string layer)
        {
            List<string> layers = new List<string>(UnityEditorInternal.InternalEditorUtility.layers);
            
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Default"  ), false, layerChangedHandler, "Default");
            
            for (int i = 0, n = layers.Count; i < n; i++)
            {
                string curLayer = layers[i];
                menu.AddItem(new GUIContent(curLayer), layer == curLayer, layerChangedHandler, curLayer);
            }
            
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Layer..."  ), false, addTagOrLayerHandler, "Layers");
            menu.ShowAsContext();
        }

        private void tagChangedHandler(object newTag)
        {
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                Undo.RecordObject(gameObject, "Change Tag");
                gameObject.tag = (string)newTag;
                EditorUtility.SetDirty(gameObject);
            }
        }

        private void layerChangedHandler(object newLayer)
        {
            int newLayerId = LayerMask.NameToLayer((string)newLayer);
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                Undo.RecordObject(gameObject, "Change Layer");
                gameObject.layer = newLayerId;
                EditorUtility.SetDirty(gameObject);
            }
        }

        private void addTagOrLayerHandler(object value)
        {
            PropertyInfo propertyInfo = typeof(EditorApplication).GetProperty("tagManager", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty);
            UnityEngine.Object obj = (UnityEngine.Object)(propertyInfo.GetValue(null, null));
            obj.GetType().GetField("m_DefaultExpandedFoldout").SetValue(obj, value);
            Selection.activeObject = obj;
        }
    }
}

