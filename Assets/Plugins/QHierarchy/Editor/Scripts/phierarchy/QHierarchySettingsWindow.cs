using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using qtools.qhierarchy.pdata;
using System;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pcomponent;

namespace qtools.qhierarchy.phierarchy
{
	public class QHierarchySettingsWindow : EditorWindow 
	{	
        // STATIC
		[MenuItem ("Tools/QHierarchy/Settings")]	
		public static void ShowWindow () 
		{ 
			EditorWindow window = EditorWindow.GetWindow(typeof(QHierarchySettingsWindow));           
            window.minSize = new Vector2(350, 50);

            #if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
                window.title = "QHierarchy";
            #else
                window.titleContent = new GUIContent("QHierarchy");
            #endif
		}

        // PRIVATE
        private bool inited = false;
        private Rect lastRect;
        private bool isProSkin;
        private int indentLevel;
        private Texture2D checkBoxChecked;
        private Texture2D checkBoxUnchecked;
        private Texture2D restoreButtonTexture;
        private Vector2 scrollPosition = new Vector2();
        private Color separatorColor;
        private Color yellowColor;
        private float totalWidth;
        private QComponentsOrderList componentsOrderList;

        // INIT
        private void init() 
        { 
            inited            = true;
            isProSkin         = EditorGUIUtility.isProSkin;
            separatorColor    = isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.59f, 0.59f, 0.59f);
            yellowColor       = isProSkin ? new Color(1.00f, 0.90f, 0.40f) : new Color(0.31f, 0.31f, 0.31f);
            checkBoxChecked   = QResources.getInstance().getTexture(QTexture.QCheckBoxChecked);
            checkBoxUnchecked = QResources.getInstance().getTexture(QTexture.QCheckBoxUnchecked);
            restoreButtonTexture = QResources.getInstance().getTexture(QTexture.QRestoreButton);
            componentsOrderList = new QComponentsOrderList(this);
        } 
         
        // GUI
		void OnGUI()
		{
            if (!inited || isProSkin != EditorGUIUtility.isProSkin)  
                init();

            indentLevel = 8;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                Rect targetRect = EditorGUILayout.GetControlRect(GUILayout.Height(0));
                if (Event.current.type == EventType.Repaint) totalWidth = targetRect.width + 8;

                this.lastRect = new Rect(0, 1, 0, 0);

                // COMPONENTS
                drawSection("COMPONENTS SETTINGS");
                float sectionStartY = lastRect.y + lastRect.height;

                drawTreeMapComponentSettings();
                drawSeparator();
                drawMonoBehaviourIconComponentSettings();
                drawSeparator();
                drawSeparatorComponentSettings();
                drawSeparator();
                drawVisibilityComponentSettings();
                drawSeparator();
                drawLockComponentSettings();
                drawSeparator();
                drawStaticComponentSettings();
                drawSeparator();
                drawErrorComponentSettings();
                drawSeparator();
                drawRendererComponentSettings();
                drawSeparator();
                drawPrefabComponentSettings();
                drawSeparator();
                drawTagLayerComponentSettings();
                drawSeparator();
                drawColorComponentSettings();
                drawSeparator();
                drawGameObjectIconComponentSettings();
                drawSeparator();
                drawTagIconComponentSettings();
                drawSeparator();
                drawLayerIconComponentSettings();
                drawSeparator();
                drawChildrenCountComponentSettings();
                drawSeparator();
                drawVerticesAndTrianglesCountComponentSettings();
                drawSeparator();
                drawComponentsComponentSettings();
                drawLeftLine(sectionStartY, lastRect.y + lastRect.height, separatorColor);

                // ORDER
                drawSection("ORDER OF COMPONENTS");         
                sectionStartY = lastRect.y + lastRect.height;
                drawSpace(8);  
                drawOrderSettings();
                drawSpace(6);      
                drawLeftLine(sectionStartY, lastRect.y + lastRect.height, separatorColor);

                // ADDITIONAL
                drawSection("ADDITIONAL SETTINGS");             
                sectionStartY = lastRect.y + lastRect.height;
                drawSpace(3);  
                drawAdditionalSettings();
                drawLeftLine(sectionStartY, lastRect.y + lastRect.height + 4, separatorColor);

                indentLevel -= 1;
            }

            EditorGUILayout.EndScrollView();
        }

        // COMPONENTS
        private void drawTreeMapComponentSettings() 
        {
            if (drawComponentCheckBox("Hierarchy Tree", QSetting.TreeMapShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.TreeMapColor);
                    QSettings.getInstance().restore(QSetting.TreeMapEnhanced);
                    QSettings.getInstance().restore(QSetting.TreeMapTransparentBackground);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawColorPicker("Tree color", QSetting.TreeMapColor);
                drawCheckBoxRight("Transparent background", QSetting.TreeMapTransparentBackground);
                drawCheckBoxRight("Enhanced (\"Transform Sort\" only)", QSetting.TreeMapEnhanced);
                drawSpace(1);
            }
        }
        
        private void drawMonoBehaviourIconComponentSettings() 
        {
            if (drawComponentCheckBox("MonoBehaviour Icon", QSetting.MonoBehaviourIconShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.MonoBehaviourIconShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.MonoBehaviourIconColor);
                    QSettings.getInstance().restore(QSetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.MonoBehaviourIconShowDuringPlayMode);
                drawColorPicker("Icon color", QSetting.MonoBehaviourIconColor);
                drawCheckBoxRight("Ignore Unity MonoBehaviours", QSetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
                drawSpace(1);
            }
        }

        private void drawSeparatorComponentSettings() 
        {
            if (drawComponentCheckBox("Separator", QSetting.SeparatorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.SeparatorColor);
                    QSettings.getInstance().restore(QSetting.SeparatorShowRowShading);
                    QSettings.getInstance().restore(QSetting.SeparatorOddRowShadingColor);
                    QSettings.getInstance().restore(QSetting.SeparatorEvenRowShadingColor);
                }
                bool rowShading = QSettings.getInstance().get<bool>(QSetting.SeparatorShowRowShading);

                drawBackground(rect.x, rect.y, rect.width, 18 * (rowShading ? 4 : 2) + 5);
                drawSpace(4);
                drawColorPicker("Separator Color", QSetting.SeparatorColor);
                drawCheckBoxRight("Row shading", QSetting.SeparatorShowRowShading);
                if (rowShading)                
                {
                    drawColorPicker("Even row shading color", QSetting.SeparatorEvenRowShadingColor);
                    drawColorPicker("Odd row shading color" , QSetting.SeparatorOddRowShadingColor);
                }
                drawSpace(1);
            }
        }

        private void drawVisibilityComponentSettings() 
        {
            if (drawComponentCheckBox("Visibility", QSetting.VisibilityShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.VisibilityShowDuringPlayMode);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.VisibilityShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private void drawLockComponentSettings() 
        {
            if (drawComponentCheckBox("Lock", QSetting.LockShow))
            {   
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.LockShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.LockPreventSelectionOfLockedObjects);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 2 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.LockShowDuringPlayMode);
                drawCheckBoxRight("Prevent selection of locked objects", QSetting.LockPreventSelectionOfLockedObjects);
                drawSpace(1);
            }
        }

        private void drawStaticComponentSettings() 
        {
            if (drawComponentCheckBox("Static", QSetting.StaticShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.StaticShowDuringPlayMode);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.StaticShowDuringPlayMode);
                drawSpace(1);
            }        
        }

        private void drawErrorComponentSettings() 
        {
            if (drawComponentCheckBox("Error", QSetting.ErrorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.ErrorShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.ErrorShowIconOnParent);
                    QSettings.getInstance().restore(QSetting.ErrorShowForDisabledComponents);
                    QSettings.getInstance().restore(QSetting.ErrorShowForDisabledGameObjects);
                    QSettings.getInstance().restore(QSetting.ErrorShowScriptIsMissing);
                    QSettings.getInstance().restore(QSetting.ErrorShowReferenceIsMissing);
                    QSettings.getInstance().restore(QSetting.ErrorShowReferenceIsNull);
                    QSettings.getInstance().restore(QSetting.ErrorShowStringIsEmpty);
                    QSettings.getInstance().restore(QSetting.ErrorShowMissingEventMethod);
                    QSettings.getInstance().restore(QSetting.ErrorShowWhenTagOrLayerIsUndefined);
                    QSettings.getInstance().restore(QSetting.ErrorIgnoreString);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 12 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.ErrorShowDuringPlayMode);
                drawCheckBoxRight("Show error icon up of hierarchy (very slow)", QSetting.ErrorShowIconOnParent);
                drawCheckBoxRight("Show error icon for disabled components", QSetting.ErrorShowForDisabledComponents);
                drawCheckBoxRight("Show error icon for disabled GameObjects", QSetting.ErrorShowForDisabledGameObjects);
                drawLabel("Show error icon for the following:");
                indentLevel += 16;
                drawCheckBoxRight("- script is missing", QSetting.ErrorShowScriptIsMissing);
                drawCheckBoxRight("- reference is missing", QSetting.ErrorShowReferenceIsMissing);
                drawCheckBoxRight("- reference is null", QSetting.ErrorShowReferenceIsNull);
                drawCheckBoxRight("- string is empty", QSetting.ErrorShowStringIsEmpty);
                drawCheckBoxRight("- callback of event is missing (very slow)", QSetting.ErrorShowMissingEventMethod);
                drawCheckBoxRight("- tag or layer is undefined", QSetting.ErrorShowWhenTagOrLayerIsUndefined);
                indentLevel -= 16;
                drawTextField("Ignore packages/classes", QSetting.ErrorIgnoreString);
                drawSpace(1);
            }
        }

        private void drawRendererComponentSettings() 
        {
            if (drawComponentCheckBox("Renderer", QSetting.RendererShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.RendererShowDuringPlayMode);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.RendererShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private void drawPrefabComponentSettings() 
        {
            if (drawComponentCheckBox("Prefab", QSetting.PrefabShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.PrefabShowBreakedPrefabsOnly);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show icon for broken prefabs only", QSetting.PrefabShowBreakedPrefabsOnly);
                drawSpace(1);
            }
        }

        private void drawTagLayerComponentSettings()
        {
            if (drawComponentCheckBox("Tag And Layer", QSetting.TagAndLayerShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.TagAndLayerShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.TagAndLayerSizeShowType);
                    QSettings.getInstance().restore(QSetting.TagAndLayerType);
                    QSettings.getInstance().restore(QSetting.TagAndLayerSizeValueType);
                    QSettings.getInstance().restore(QSetting.TagAndLayerSizeValuePixel);
                    QSettings.getInstance().restore(QSetting.TagAndLayerSizeValuePercent);
                    QSettings.getInstance().restore(QSetting.TagAndLayerAligment);
                    QSettings.getInstance().restore(QSetting.TagAndLayerLabelSize);
                    QSettings.getInstance().restore(QSetting.TagAndLayerLabelAlpha);
                    QSettings.getInstance().restore(QSetting.TagAndLayerTagLabelColor);
                    QSettings.getInstance().restore(QSetting.TagAndLayerLayerLabelColor);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 10 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.TagAndLayerShowDuringPlayMode);  
                drawEnum("Show", QSetting.TagAndLayerSizeShowType, typeof(QHierarchyTagAndLayerShowType));
                drawEnum("Show tag and layer", QSetting.TagAndLayerType, typeof(QHierarchyTagAndLayerType));

                QHierarchyTagAndLayerSizeType newTagAndLayerSizeValueType = (QHierarchyTagAndLayerSizeType)drawEnum("Unit of width", QSetting.TagAndLayerSizeValueType, typeof(QHierarchyTagAndLayerSizeType));               

                if (newTagAndLayerSizeValueType == QHierarchyTagAndLayerSizeType.Pixel)                
                    drawIntSlider("Width in pixels", QSetting.TagAndLayerSizeValuePixel, 5, 250);                
                else                
                    drawFloatSlider("Percentage width", QSetting.TagAndLayerSizeValuePercent, 0, 0.5f);
                           
                drawEnum("Alignment", QSetting.TagAndLayerAligment, typeof(QHierarchyTagAndLayerAligment));
                drawEnum("Label size", QSetting.TagAndLayerLabelSize, typeof(QHierarchyTagAndLayerLabelSize));
                drawFloatSlider("Label alpha if default", QSetting.TagAndLayerLabelAlpha, 0, 1.0f);
                drawColorPicker("Tag label color", QSetting.TagAndLayerTagLabelColor);
                drawColorPicker("Layer label color", QSetting.TagAndLayerLayerLabelColor);
                drawSpace(1);
            }
        }

        private void drawColorComponentSettings() 
        {
            if (drawComponentCheckBox("Color", QSetting.ColorShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.ColorShowDuringPlayMode);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.ColorShowDuringPlayMode);
                drawSpace(1);
            }
        }

        private void drawGameObjectIconComponentSettings() 
        {
            if (drawComponentCheckBox("GameObject Icon", QSetting.GameObjectIconShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.GameObjectIconShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.GameObjectIconSize);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 2 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.GameObjectIconShowDuringPlayMode);
                drawEnum("Icon size", QSetting.GameObjectIconSize, typeof(QHierarchySizeAll));
                drawSpace(1);
            }
        }

        private void drawTagIconComponentSettings() 
        {
            if (drawComponentCheckBox("Tag Icon", QSetting.TagIconShow))
            {     
                string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

                bool showTagIconList = QSettings.getInstance().get<bool>(QSetting.TagIconListFoldout);

                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.TagIconShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.TagIconSize);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + (showTagIconList ? 18 * tags.Length : 0) + 4 + 5);    

                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.TagIconShowDuringPlayMode);
                drawEnum("Icon size", QSetting.TagIconSize, typeof(QHierarchySizeAll));
                drawSpace(4);
                if (drawFoldout("Tag icon list", QSetting.TagIconListFoldout))
                {
                    List<QTagTexture> tagTextureList = QTagTexture.loadTagTextureList();
                
                    bool changed = false;
                    for (int i = 0; i < tags.Length; i++) 
                    {
                        string tag = tags[i];
                        QTagTexture tagTexture = tagTextureList.Find(t => t.tag == tag);
                        Texture2D newTexture = (Texture2D)EditorGUI.ObjectField(getControlRect(0, 16, 31, 6), 
                                                                                tag, tagTexture == null ? null : tagTexture.texture, typeof(Texture2D), false);
                        if (newTexture != null && tagTexture == null)
                        {
                            QTagTexture newTagTexture = new QTagTexture(tag, newTexture);
                            tagTextureList.Add(newTagTexture);
                            
                            changed = true;
                        }
                        else if (newTexture == null && tagTexture != null)
                        {
                            tagTextureList.Remove(tagTexture);                            
                            changed = true;
                        }
                        else if (tagTexture != null && tagTexture.texture != newTexture)
                        {
                            tagTexture.texture = newTexture;
                            changed = true;
                        }

                        drawSpace(i == tags.Length - 1 ? 2 : 2);
                    }                 

                    if (changed) 
                    {     
                        QTagTexture.saveTagTextureList(QSetting.TagIconList, tagTextureList);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }

                drawSpace(1);
            }
        }

        private void drawLayerIconComponentSettings() 
        {
            if (drawComponentCheckBox("Layer Icon", QSetting.LayerIconShow))
            {     
                string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
                
                bool showLayerIconList = QSettings.getInstance().get<bool>(QSetting.LayerIconListFoldout);
                
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.LayerIconShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.LayerIconSize);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + (showLayerIconList ? 18 * layers.Length : 0) + 4 + 5);    
                
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.LayerIconShowDuringPlayMode);
                drawEnum("Icon size", QSetting.LayerIconSize, typeof(QHierarchySizeAll));
                drawSpace(4);
                if (drawFoldout("Layer icon list", QSetting.LayerIconListFoldout))
                {
                    List<QLayerTexture> layerTextureList = QLayerTexture.loadLayerTextureList();
                    
                    bool changed = false;
                    for (int i = 0; i < layers.Length; i++) 
                    {
                        string layer = layers[i];
                        QLayerTexture layerTexture = layerTextureList.Find(t => t.layer == layer);
                        Texture2D newTexture = (Texture2D)EditorGUI.ObjectField(getControlRect(0, 16, 31, 6), 
                                                                                layer, layerTexture == null ? null : layerTexture.texture, typeof(Texture2D), false);
                        if (newTexture != null && layerTexture == null)
                        {
                            QLayerTexture newLayerTexture = new QLayerTexture(layer, newTexture);
                            layerTextureList.Add(newLayerTexture);
                            
                            changed = true;
                        }
                        else if (newTexture == null && layerTexture != null)
                        {
                            layerTextureList.Remove(layerTexture);                            
                            changed = true;
                        }
                        else if (layerTexture != null && layerTexture.texture != newTexture)
                        {
                            layerTexture.texture = newTexture;
                            changed = true;
                        }
                        
                        drawSpace(i == layers.Length - 1 ? 2 : 2);
                    }                 
                    
                    if (changed) 
                    {     
                        QLayerTexture.saveLayerTextureList(QSetting.LayerIconList, layerTextureList);
                        EditorApplication.RepaintHierarchyWindow();
                    }
                }
                
                drawSpace(1);
            }
        }

        private void drawChildrenCountComponentSettings() 
        {
            if (drawComponentCheckBox("Children Count", QSetting.ChildrenCountShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.ChildrenCountShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.ChildrenCountLabelSize);
                    QSettings.getInstance().restore(QSetting.ChildrenCountLabelColor);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.ChildrenCountShowDuringPlayMode);
                drawEnum("Label size", QSetting.ChildrenCountLabelSize, typeof(QHierarchySize));
                drawColorPicker("Label color", QSetting.ChildrenCountLabelColor);
                drawSpace(1);
            }
        }
        
        private void drawVerticesAndTrianglesCountComponentSettings()
        {
            if (drawComponentCheckBox("Vertices And Triangles Count", QSetting.VerticesAndTrianglesShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesShowVertices);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesShowTriangles);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesCalculateTotalCount);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesLabelSize);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesVerticesLabelColor);
                    QSettings.getInstance().restore(QSetting.VerticesAndTrianglesTrianglesLabelColor);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 7 + 5);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.VerticesAndTrianglesShowDuringPlayMode);                   
                if (drawCheckBoxRight("Show vertices count", QSetting.VerticesAndTrianglesShowVertices))
                {
                    if (QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowVertices) == false &&
                        QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowTriangles) == false)                    
                        QSettings.getInstance().set(QSetting.VerticesAndTrianglesShowTriangles, true);
                }
                if (drawCheckBoxRight("Show triangles count (very slow)", QSetting.VerticesAndTrianglesShowTriangles))
                {
                    if (QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowVertices) == false &&
                        QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowTriangles) == false)                 
                        QSettings.getInstance().set(QSetting.VerticesAndTrianglesShowVertices, true);
                }
                drawCheckBoxRight("Calculate the count including children (very slow)", QSetting.VerticesAndTrianglesCalculateTotalCount);
                drawEnum("Label size", QSetting.VerticesAndTrianglesLabelSize, typeof(QHierarchySize));
                drawColorPicker("Vertices label color", QSetting.VerticesAndTrianglesVerticesLabelColor);
                drawColorPicker("Triangles label color", QSetting.VerticesAndTrianglesTrianglesLabelColor);
                drawSpace(1);
            }
        }

        private void drawComponentsComponentSettings() 
        {
            if (drawComponentCheckBox("Components", QSetting.ComponentsShow))
            {
                Rect rect = getControlRect(0, 0);
                if (drawRestore())
                {
                    QSettings.getInstance().restore(QSetting.ComponentsShowDuringPlayMode);
                    QSettings.getInstance().restore(QSetting.ComponentsIconSize);
                }
                drawBackground(rect.x, rect.y, rect.width, 18 * 3 + 6);
                drawSpace(4);
                drawCheckBoxRight("Show component during play mode", QSetting.ComponentsShowDuringPlayMode);
                drawEnum("Icon size", QSetting.ComponentsIconSize, typeof(QHierarchySizeAll));
                drawTextField("Ignore packages/classes", QSetting.ComponentsIgnore);
                drawSpace(2);
            }
        }

        // COMPONENTS ORDER
        private void drawOrderSettings()
        {
            if (drawRestore())
            {
                QSettings.getInstance().restore(QSetting.ComponentsOrder);
            }

            indentLevel += 4;
            
            string componentOrder = QSettings.getInstance().get<string>(QSetting.ComponentsOrder);
            string[] componentIds = componentOrder.Split(';');
            
            Rect rect = getControlRect(position.width, 17 * componentIds.Length + 10, 0, 0);
            if (componentsOrderList == null) 
                componentsOrderList = new QComponentsOrderList(this);
            componentsOrderList.draw(rect, componentIds);
            
            indentLevel -= 4;
        }  

        // ADDITIONAL SETTINGS
        private void drawAdditionalSettings()
        {
            if (drawRestore())
            {
                QSettings.getInstance().restore(QSetting.AdditionalShowHiddenQHierarchyObjectList);
                QSettings.getInstance().restore(QSetting.AdditionalHideIconsIfNotFit);
                QSettings.getInstance().restore(QSetting.AdditionalIdentation);
                QSettings.getInstance().restore(QSetting.AdditionalShowModifierWarning);
                QSettings.getInstance().restore(QSetting.AdditionalBackgroundColor);
                QSettings.getInstance().restore(QSetting.AdditionalActiveColor);
                QSettings.getInstance().restore(QSetting.AdditionalInactiveColor);
                QSettings.getInstance().restore(QSetting.AdditionalSpecialColor);
            }
            drawSpace(4);
            drawCheckBoxRight("Show QHierarchyObjectList GameObject", QSetting.AdditionalShowHiddenQHierarchyObjectList);
            drawCheckBoxRight("Hide icons if not fit", QSetting.AdditionalHideIconsIfNotFit);
            drawIntSlider("Right indent", QSetting.AdditionalIdentation, 0, 500);      
            drawCheckBoxRight("Show warning when using modifiers + click", QSetting.AdditionalShowModifierWarning);
            drawColorPicker("Background color", QSetting.AdditionalBackgroundColor);
            drawColorPicker("Active color", QSetting.AdditionalActiveColor);
            drawColorPicker("Inactive color", QSetting.AdditionalInactiveColor);
            drawColorPicker("Special color", QSetting.AdditionalSpecialColor);
            drawSpace(1);
        }

        // PRIVATE
        private void drawSection(string title)
        {
            Rect rect = getControlRect(0, 24, -3, 0);
            rect.width *= 2;
            rect.x = 0;
            GUI.Box(rect, "");             
            
            drawLeftLine(rect.y, rect.y + 24, yellowColor);
            
            rect.x = lastRect.x + 8;
            rect.y += 4;
            GUI.Label(rect, title);
        }

        private void drawSeparator(int spaceBefore = 0, int spaceAfter = 0, int height = 1)
        {
            if (spaceBefore > 0) drawSpace(spaceBefore);
            Rect rect = getControlRect(0, height, 0, 0);
            rect.width += 8;
            EditorGUI.DrawRect(rect, separatorColor);
            if (spaceAfter > 0) drawSpace(spaceAfter);
        }

        private bool drawComponentCheckBox(string label, QSetting setting)
        {
            indentLevel += 8;

            Rect rect = getControlRect(0, 28, 0, 0);

            float rectWidth = rect.width;
            bool isChecked = QSettings.getInstance().get<bool>(setting);

            rect.x -= 1;
            rect.y += 7;
            rect.width  = 14;
            rect.height = 14;

            if (GUI.Button(rect, isChecked ? checkBoxChecked : checkBoxUnchecked, GUIStyle.none))
            {
                QSettings.getInstance().set(setting, !isChecked);
            }

            rect.x += 14 + 10;
            rect.width = rectWidth - 14 - 8;
            rect.y -= 1;
            rect.height = 20;

            GUI.Label(rect, label);

            indentLevel -= 8;

            return isChecked;
        }

        private bool drawCheckBoxRight(string label, QSetting setting)
        {
            Rect rect = getControlRect(0, 18, 31, 6);
            bool result = false;
            bool isChecked = QSettings.getInstance().get<bool>(setting);

            float tempX = rect.x;
            rect.x += rect.width - 14;
            rect.y += 1;
            rect.width  = 14;
            rect.height = 14;
            
            if (GUI.Button(rect, isChecked ? checkBoxChecked : checkBoxUnchecked, GUIStyle.none))
            {
                QSettings.getInstance().set(setting, !isChecked);
                result = true;
            }

            rect.width = rect.x - tempX - 4;
            rect.x = tempX;
            rect.y -= 1;
            rect.height = 18;
            
            GUI.Label(rect, label);
            
            return result;
        }

        private void drawSpace(int value)
        {
            getControlRect(0, value, 0, 0);
        }

        private void drawBackground(float x, float y, float width, float height)
        {
            EditorGUI.DrawRect(new Rect(x, y, width, height), separatorColor);
        }
        
        private void drawLeftLine(float fromY, float toY, Color color, float width = 0)
        {
            EditorGUI.DrawRect(new Rect(0, fromY, width == 0 ? indentLevel : width, toY - fromY), color);
        }
        
        private Rect getControlRect(float width, float height, float addIndent = 0, float remWidth = 0)
        { 
            EditorGUILayout.GetControlRect(false, height, GUIStyle.none, GUILayout.ExpandWidth(true));
            Rect rect = new Rect(indentLevel + addIndent, lastRect.y + lastRect.height, (width == 0 ? totalWidth - indentLevel - addIndent - remWidth: width), height);
            lastRect = rect;
            return rect;
        }

        private bool drawRestore()
        {
            if (GUI.Button(new Rect(lastRect.x + lastRect.width - 16 - 5, lastRect.y - 20, 16, 16), restoreButtonTexture, GUIStyle.none))
            {
                if (EditorUtility.DisplayDialog("Restore", "Restore default settings?", "Ok", "Cancel"))
                {
                    return true;
                }
            }
            return false;
        }

        // GUI COMPONENTS
        private void drawLabel(string label)
        {
            EditorGUI.LabelField(getControlRect(0, 16, 31, 6), label);
            drawSpace(2);
        }

        private void drawTextField(string label, QSetting setting)
        {
            string currentValue = QSettings.getInstance().get<string>(setting);
            string newValue     = EditorGUI.TextField(getControlRect(0, 16, 31, 6), label, currentValue);
            if (!currentValue.Equals(newValue)) QSettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }

        private bool drawFoldout(string label, QSetting setting)
        {
            bool foldoutValue = QSettings.getInstance().get<bool>(setting);
            bool newFoldoutValue = EditorGUI.Foldout(getControlRect(0, 16, 19, 6), foldoutValue, label);
            if (foldoutValue != newFoldoutValue) QSettings.getInstance().set(setting, newFoldoutValue);
            drawSpace(2);
            return newFoldoutValue;
        }

        private void drawColorPicker(string label, QSetting setting)
        {
            Color currentColor = QSettings.getInstance().getColor(setting);
            Color newColor = EditorGUI.ColorField(getControlRect(0, 16, 31, 6), label, currentColor);
            if (!currentColor.Equals(newColor)) QSettings.getInstance().setColor(setting, newColor);
            drawSpace(2);
        }

        private Enum drawEnum(string label, QSetting setting, Type enumType)
        {
            Enum currentEnum = (Enum)Enum.ToObject(enumType, QSettings.getInstance().get<int>(setting));
            Enum newEnumValue;                      
            if (!(newEnumValue = EditorGUI.EnumPopup(getControlRect(0, 16, 31, 6), label, currentEnum)).Equals(currentEnum))                
                QSettings.getInstance().set(setting, (int)(object)newEnumValue);                  
            drawSpace(2);
            return newEnumValue;
        }

        private void drawIntSlider(string label, QSetting setting, int minValue, int maxValue)
        {
            Rect rect = getControlRect(0, 16, 31, 6);
            int currentValue = QSettings.getInstance().get<int>(setting);
            int newValue = EditorGUI.IntSlider(rect, label, currentValue, minValue, maxValue);
            if (currentValue != newValue) QSettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }

        private void drawFloatSlider(string label, QSetting setting, float minValue, float maxValue)
        {
            Rect rect = getControlRect(0, 16, 31, 6);
            float currentValue = QSettings.getInstance().get<float>(setting);
            float newValue = EditorGUI.Slider(rect, label, currentValue, minValue, maxValue);
            if (currentValue != newValue) QSettings.getInstance().set(setting, newValue);
            drawSpace(2);
        }
	}
}