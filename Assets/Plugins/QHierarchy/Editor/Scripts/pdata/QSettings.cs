using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using System.Text;

namespace qtools.qhierarchy.pdata
{
	public enum QSetting
	{
		TreeMapShow                                 = 0,
        TreeMapColor                                = 77,
        TreeMapEnhanced                             = 78,
        TreeMapTransparentBackground                = 60,

        MonoBehaviourIconShow                       = 4,
        MonoBehaviourIconShowDuringPlayMode         = 18,
        MonoBehaviourIconIgnoreUnityMonobehaviour   = 45,
        MonoBehaviourIconColor                      = 82,

        SeparatorShow                               = 8,
        SeparatorShowRowShading                     = 50,
        SeparatorColor                              = 80,
        SeparatorEvenRowShadingColor                = 79,       
        SeparatorOddRowShadingColor                 = 81,       

		VisibilityShow                              = 1,
        VisibilityShowDuringPlayMode                = 15,

		LockShow                                    = 2,
        LockShowDuringPlayMode                      = 16,
        LockPreventSelectionOfLockedObjects         = 41,

        StaticShow                                  = 12,
        StaticShowDuringPlayMode                    = 25,

        ErrorShow                                   = 6,
        ErrorShowDuringPlayMode                     = 20,
        ErrorShowIconOnParent                       = 27,
        ErrorShowScriptIsMissing                    = 28,
        ErrorShowReferenceIsNull                    = 29,
        ErrorShowReferenceIsMissing                 = 58,
        ErrorShowStringIsEmpty                      = 30,
        ErrorShowMissingEventMethod                 = 31,
        ErrorShowWhenTagOrLayerIsUndefined          = 32,
        ErrorIgnoreString                           = 33,
        ErrorShowForDisabledComponents              = 44,
        ErrorShowForDisabledGameObjects             = 59,

        RendererShow                                = 7,
        RendererShowDuringPlayMode                  = 21,

        PrefabShow                                  = 13,
        PrefabShowBreakedPrefabsOnly                = 51,

		TagAndLayerShow                             = 5,
        TagAndLayerShowDuringPlayMode               = 19,
        TagAndLayerSizeShowType                     = 68,
        TagAndLayerType                             = 34,
        TagAndLayerSizeType                         = 35,
        TagAndLayerSizeValuePixel                   = 36,
        TagAndLayerAligment                         = 37,
        TagAndLayerSizeValueType                    = 46,
        TagAndLayerSizeValuePercent                 = 47,
        TagAndLayerLabelSize                        = 48,
        TagAndLayerTagLabelColor                    = 66,
        TagAndLayerLayerLabelColor                  = 67,
        TagAndLayerLabelAlpha                       = 69,

        ColorShow                                   = 9,
        ColorShowDuringPlayMode                     = 22,

        GameObjectIconShow                          = 3,
        GameObjectIconShowDuringPlayMode            = 17,
        GameObjectIconSize                          = 63,

        TagIconShow                                 = 14,
        TagIconShowDuringPlayMode                   = 26,
        TagIconListFoldout                          = 84,
        TagIconList                                 = 40,
        TagIconSize                                 = 62,

        LayerIconShow                               = 85,
        LayerIconShowDuringPlayMode                 = 86,
        LayerIconListFoldout                        = 87,
        LayerIconList                               = 88,
        LayerIconSize                               = 89,

        ChildrenCountShow                           = 11,
        ChildrenCountShowDuringPlayMode             = 24,
        ChildrenCountLabelSize                      = 61,
        ChildrenCountLabelColor                     = 70,

        VerticesAndTrianglesShow                    = 53,
        VerticesAndTrianglesShowDuringPlayMode      = 54,
        VerticesAndTrianglesCalculateTotalCount     = 55,
        VerticesAndTrianglesShowTriangles           = 56, 
        VerticesAndTrianglesShowVertices            = 64, 
        VerticesAndTrianglesLabelSize               = 57,
        VerticesAndTrianglesVerticesLabelColor      = 71,
        VerticesAndTrianglesTrianglesLabelColor     = 72,

        ComponentsShow                              = 10,
        ComponentsShowDuringPlayMode                = 23,
        ComponentsIconSize                          = 65,
        ComponentsIgnore                            = 90,

		ComponentsOrder                             = 38,

        AdditionalIdentation                        = 39,
        AdditionalShowHiddenQHierarchyObjectList    = 42,
        AdditionalShowModifierWarning               = 43,
        AdditionalShowObjectListContent             = 49,
        AdditionalHideIconsIfNotFit                 = 52,  
        AdditionalBackgroundColor                   = 73,
        AdditionalActiveColor                       = 74,
        AdditionalInactiveColor                     = 75,
        AdditionalSpecialColor                      = 76,
	}
	
	public enum QHierarchyTagAndLayerType
	{
		Always           = 0,
		OnlyIfNotDefault = 1
	}

    public enum QHierarchyTagAndLayerShowType
    {
        TagAndLayer = 0,
        Tag         = 1,
        Layer       = 2
    }

    public enum QHierarchyTagAndLayerAligment
    {
        Left   = 0,
        Center = 1,
        Right  = 2
    }

    public enum QHierarchyTagAndLayerSizeType
    {
        Pixel   = 0,
        Percent = 1
    }

    public enum QHierarchyTagAndLayerLabelSize
    {
        Normal                          = 0,
        Big                             = 1,
        BigIfSpecifiedOnlyTagOrLayer    = 2
    }

    public enum QHierarchySize
    {
        Normal  = 0,
        Big     = 1
    }
        
    public enum QHierarchySizeAll
    {
        Small   = 0,
        Normal  = 1,
        Big     = 2
    }

	public enum QHierarchyComponentEnum
	{
        LockComponent               = 0,
        VisibilityComponent         = 1,
        StaticComponent             = 2,
        ColorComponent              = 3,
        ErrorComponent              = 4,
        RendererComponent           = 5,
        PrefabComponent             = 6,
        TagAndLayerComponent        = 7,
        GameObjectIconComponent     = 8,
        TagIconComponent            = 9,
        LayerIconComponent          = 10,
        ChildrenCountComponent      = 11,
        VerticesAndTrianglesCount   = 12,
        SeparatorComponent          = 1000,
        TreeMapComponent            = 1001,
        MonoBehaviourIconComponent  = 1002,
        ComponentsComponent         = 1003
	}

    public class QTagTexture
    {
        public string tag;
        public Texture2D texture;
        
        public QTagTexture(string tag, Texture2D texture)
        {
            this.tag = tag;
            this.texture = texture;
        }

        public static List<QTagTexture> loadTagTextureList()
        {
            List<QTagTexture> tagTextureList = new List<QTagTexture>();
            string customTagIcon = QSettings.getInstance().get<string>(QSetting.TagIconList);
            string[] customTagIconArray = customTagIcon.Split(new char[]{';'});
            List<string> tags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            for (int i = 0; i < customTagIconArray.Length - 1; i+=2)
            {
                string tag = customTagIconArray[i];
                if (!tags.Contains(tag)) continue;
                string texturePath = customTagIconArray[i+1];
                
                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                if (texture != null) 
                { 
                    QTagTexture tagTexture = new QTagTexture(tag, texture);
                    tagTextureList.Add(tagTexture);
                }  
            }
            return tagTextureList;
        }

        public static void saveTagTextureList(QSetting setting, List<QTagTexture> tagTextureList)
        { 
            string result = "";
            for (int i = 0; i < tagTextureList.Count; i++)            
                result += tagTextureList[i].tag + ";" + AssetDatabase.GetAssetPath(tagTextureList[i].texture.GetInstanceID()) + ";";
            QSettings.getInstance().set(setting, result);
        }
    }

    public class QLayerTexture
    {
        public string layer;
        public Texture2D texture;
        
        public QLayerTexture(string layer, Texture2D texture)
        {
            this.layer = layer;
            this.texture = texture;
        }
        
        public static List<QLayerTexture> loadLayerTextureList()
        {
            List<QLayerTexture> layerTextureList = new List<QLayerTexture>();
            string customTagIcon = QSettings.getInstance().get<string>(QSetting.LayerIconList);
            string[] customLayerIconArray = customTagIcon.Split(new char[]{';'});
            List<string> layers = new List<string>(UnityEditorInternal.InternalEditorUtility.layers);
            for (int i = 0; i < customLayerIconArray.Length - 1; i+=2)
            {
                string layer = customLayerIconArray[i];
                if (!layers.Contains(layer)) continue;
                string texturePath = customLayerIconArray[i+1];
                
                Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                if (texture != null) 
                { 
                    QLayerTexture tagTexture = new QLayerTexture(layer, texture);
                    layerTextureList.Add(tagTexture);
                }  
            }
            return layerTextureList;
        }
        
        public static void saveLayerTextureList(QSetting setting, List<QLayerTexture> layerTextureList)
        { 
            string result = "";
            for (int i = 0; i < layerTextureList.Count; i++)            
                result += layerTextureList[i].layer + ";" + AssetDatabase.GetAssetPath(layerTextureList[i].texture.GetInstanceID()) + ";";
            QSettings.getInstance().set(setting, result);
        }
    }

    public delegate void QSettingChangedHandler();

	public class QSettings 
	{
        // CONST
		private const string PREFS_PREFIX = "QTools.QHierarchy_";
        private const string PREFS_DARK = "Dark_";
        private const string PREFS_LIGHT = "Light_";
        public const string DEFAULT_ORDER = "0;1;2;3;4;5;6;7;8;9;10;11;12";
        public const int DEFAULT_ORDER_COUNT = 13;
        private const string SETTINGS_FILE_NAME = "QSettingsObjectAsset";

        // PRIVATE
        private QSettingsObject settingsObject;
        private Dictionary<int, object> defaultSettings = new Dictionary<int, object>();
        private HashSet<int> skinDependedSettings = new HashSet<int>();
        private Dictionary<int, QSettingChangedHandler> settingChangedHandlerList = new Dictionary<int, QSettingChangedHandler>();

        // SINGLETON
        private static QSettings instance;
        public static QSettings getInstance()
        {
            if (instance == null) instance = new QSettings();
            return instance;
        }

        // CONSTRUCTOR
		private QSettings()
		{ 
            string[] paths = AssetDatabase.FindAssets(SETTINGS_FILE_NAME); 
            for (int i = 0; i < paths.Length; i++)
            {
                settingsObject = (QSettingsObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(paths[i]), typeof(QSettingsObject));
                if (settingsObject != null) break;
            }
            if (settingsObject == null) 
            {
                settingsObject = ScriptableObject.CreateInstance<QSettingsObject>();
                string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(settingsObject));
                path = path.Substring(0, path.LastIndexOf("/"));
                AssetDatabase.CreateAsset(settingsObject, path + "/" + SETTINGS_FILE_NAME + ".asset");
                AssetDatabase.SaveAssets();
            }  

            initSetting(QSetting.TreeMapShow                                , true);
            initSetting(QSetting.TreeMapColor                               , "39FFFFFF", "99444444");
            initSetting(QSetting.TreeMapEnhanced                            , false);
            initSetting(QSetting.TreeMapTransparentBackground               , false);

            initSetting(QSetting.MonoBehaviourIconShow                      , true);
            initSetting(QSetting.MonoBehaviourIconShowDuringPlayMode        , true);
            initSetting(QSetting.MonoBehaviourIconIgnoreUnityMonobehaviour  , true);
            initSetting(QSetting.MonoBehaviourIconColor                     , "FF1B6DBB");

            initSetting(QSetting.SeparatorShow                              , true);
            initSetting(QSetting.SeparatorShowRowShading                    , true);
            initSetting(QSetting.SeparatorColor                             , "FF303030", "88666666");
            initSetting(QSetting.SeparatorEvenRowShadingColor               , "13000000", "18000000");
            initSetting(QSetting.SeparatorOddRowShadingColor                , "00000000", "00FFFFFF");

            initSetting(QSetting.VisibilityShow                             , true);
            initSetting(QSetting.VisibilityShowDuringPlayMode               , true);

            initSetting(QSetting.LockShow                                   , true);
            initSetting(QSetting.LockShowDuringPlayMode                     , false);
            initSetting(QSetting.LockPreventSelectionOfLockedObjects        , false);

            initSetting(QSetting.StaticShow                                 , true); 
            initSetting(QSetting.StaticShowDuringPlayMode                   , false);

            initSetting(QSetting.ErrorShow                                  , true);
            initSetting(QSetting.ErrorShowDuringPlayMode                    , false);
            initSetting(QSetting.ErrorShowIconOnParent                      , false);
            initSetting(QSetting.ErrorShowScriptIsMissing                   , true);
            initSetting(QSetting.ErrorShowReferenceIsNull                   , false);
            initSetting(QSetting.ErrorShowReferenceIsMissing                , true);
            initSetting(QSetting.ErrorShowStringIsEmpty                     , false);
            initSetting(QSetting.ErrorShowMissingEventMethod                , true);
            initSetting(QSetting.ErrorShowWhenTagOrLayerIsUndefined         , true);
            initSetting(QSetting.ErrorIgnoreString                          , "");
            initSetting(QSetting.ErrorShowForDisabledComponents             , true);
            initSetting(QSetting.ErrorShowForDisabledGameObjects            , true);

            initSetting(QSetting.RendererShow                               , false);
            initSetting(QSetting.RendererShowDuringPlayMode                 , false);

            initSetting(QSetting.PrefabShow                                 , false);
            initSetting(QSetting.PrefabShowBreakedPrefabsOnly               , false);

            initSetting(QSetting.TagAndLayerShow                            , true);
            initSetting(QSetting.TagAndLayerShowDuringPlayMode              , true);
            initSetting(QSetting.TagAndLayerSizeShowType                    , (int)QHierarchyTagAndLayerShowType.TagAndLayer);
            initSetting(QSetting.TagAndLayerType                            , (int)QHierarchyTagAndLayerType.OnlyIfNotDefault);
            initSetting(QSetting.TagAndLayerAligment                        , (int)QHierarchyTagAndLayerAligment.Left);
            initSetting(QSetting.TagAndLayerSizeValueType                   , (int)QHierarchyTagAndLayerSizeType.Pixel);
            initSetting(QSetting.TagAndLayerSizeValuePercent                , 0.25f);
            initSetting(QSetting.TagAndLayerSizeValuePixel                  , 75);
            initSetting(QSetting.TagAndLayerLabelSize                       , (int)QHierarchyTagAndLayerLabelSize.Normal);
            initSetting(QSetting.TagAndLayerTagLabelColor                   , "FFCCCCCC", "FF333333");
            initSetting(QSetting.TagAndLayerLayerLabelColor                 , "FFCCCCCC", "FF333333");
            initSetting(QSetting.TagAndLayerLabelAlpha                      , 0.35f);

            initSetting(QSetting.ColorShow                                  , true);
            initSetting(QSetting.ColorShowDuringPlayMode                    , true);

            initSetting(QSetting.GameObjectIconShow                         , false);
            initSetting(QSetting.GameObjectIconShowDuringPlayMode           , true);
            initSetting(QSetting.GameObjectIconSize                         , (int)QHierarchySizeAll.Small);

            initSetting(QSetting.TagIconShow                                , false);
            initSetting(QSetting.TagIconShowDuringPlayMode                  , true);
            initSetting(QSetting.TagIconListFoldout                         , false);
            initSetting(QSetting.TagIconList                                , "");
            initSetting(QSetting.TagIconSize                                , (int)QHierarchySizeAll.Small);

            initSetting(QSetting.LayerIconShow                              , false);
            initSetting(QSetting.LayerIconShowDuringPlayMode                , true);
            initSetting(QSetting.LayerIconListFoldout                       , false);
            initSetting(QSetting.LayerIconList                              , "");
            initSetting(QSetting.LayerIconSize                              , (int)QHierarchySizeAll.Small);

            initSetting(QSetting.ChildrenCountShow                          , false);     
            initSetting(QSetting.ChildrenCountShowDuringPlayMode            , true);
            initSetting(QSetting.ChildrenCountLabelSize                     , (int)QHierarchySize.Normal);
            initSetting(QSetting.ChildrenCountLabelColor                    , "FFCCCCCC", "FF333333");

            initSetting(QSetting.VerticesAndTrianglesShow                   , false);
            initSetting(QSetting.VerticesAndTrianglesShowDuringPlayMode     , false);
            initSetting(QSetting.VerticesAndTrianglesCalculateTotalCount    , false);
            initSetting(QSetting.VerticesAndTrianglesShowTriangles          , false);
            initSetting(QSetting.VerticesAndTrianglesShowVertices           , true);
            initSetting(QSetting.VerticesAndTrianglesLabelSize              , (int)QHierarchySize.Normal);
            initSetting(QSetting.VerticesAndTrianglesVerticesLabelColor     , "FFCCCCCC", "FF333333");
            initSetting(QSetting.VerticesAndTrianglesTrianglesLabelColor    , "FFCCCCCC", "FF333333");

            initSetting(QSetting.ComponentsShow                             , false);
            initSetting(QSetting.ComponentsShowDuringPlayMode               , false);
            initSetting(QSetting.ComponentsIconSize                         , (int)QHierarchySizeAll.Small);
            initSetting(QSetting.ComponentsIgnore                           , "");

            initSetting(QSetting.ComponentsOrder                            , DEFAULT_ORDER);

            initSetting(QSetting.AdditionalShowObjectListContent            , false);
            initSetting(QSetting.AdditionalShowHiddenQHierarchyObjectList   , true);
            initSetting(QSetting.AdditionalHideIconsIfNotFit                , true);
            initSetting(QSetting.AdditionalIdentation                       , 0);
            initSetting(QSetting.AdditionalShowModifierWarning              , true);

            initSetting(QSetting.AdditionalBackgroundColor                  , "FF383838", "FFC2C2C2");
            initSetting(QSetting.AdditionalActiveColor                      , "FFFFFF80", "CF363636");
            initSetting(QSetting.AdditionalInactiveColor                    , "FF4F4F4F", "1E000000");
            initSetting(QSetting.AdditionalSpecialColor                     , "FF2CA8CA", "FF1D78D5");
		} 

        // DESTRUCTOR
        public void OnDestroy()
        {
            skinDependedSettings = null;
            defaultSettings = null;
            settingsObject = null;
            settingChangedHandlerList = null;
            instance = null;
        }

        // PUBLIC
        public T get<T>(QSetting setting)
        {
            return (T)settingsObject.get<T>(getSettingName(setting));
        }

        public Color getColor(QSetting setting)
        {
            string stringColor = (string)settingsObject.get<string>(getSettingName(setting));
            return QColorUtils.fromString(stringColor);
        }

        public void setColor(QSetting setting, Color color)
        {
            string stringColor = QColorUtils.toString(color);
            set(setting, stringColor);
        }

        public void set<T>(QSetting setting, T value, bool invokeChanger = true)
        {
            int settingId = (int)setting;
            settingsObject.set(getSettingName(setting), value);

            if (invokeChanger && settingChangedHandlerList.ContainsKey(settingId) && settingChangedHandlerList[settingId] != null)            
                settingChangedHandlerList[settingId].Invoke();
            
            EditorApplication.RepaintHierarchyWindow();
        }

        public void addEventListener(QSetting setting, QSettingChangedHandler handler)
        {
            int settingId = (int)setting;
            
            if (!settingChangedHandlerList.ContainsKey(settingId))          
                settingChangedHandlerList.Add(settingId, null);
            
            if (settingChangedHandlerList[settingId] == null)           
                settingChangedHandlerList[settingId] = handler;
            else            
                settingChangedHandlerList[settingId] += handler;
        }
        
        public void removeEventListener(QSetting setting, QSettingChangedHandler handler)
        {
            int settingId = (int)setting;
            
            if (settingChangedHandlerList.ContainsKey(settingId) && settingChangedHandlerList[settingId] != null)       
                settingChangedHandlerList[settingId] -= handler;
        }
        
        public void restore(QSetting setting)
        {
            set(setting, defaultSettings[(int)setting]);
        }

        // PRIVATE
        private void initSetting(QSetting setting, object defaultValueDark, object defaultValueLight)
        {
            skinDependedSettings.Add((int)setting);
            initSetting(setting, EditorGUIUtility.isProSkin ? defaultValueDark : defaultValueLight);
        }
        
        private void initSetting(QSetting setting, object defaultValue)
        {
            string settingName = getSettingName(setting);
            defaultSettings.Add((int)setting, defaultValue);
            object value = settingsObject.get(settingName, defaultValue);
            if (value == null || value.GetType() != defaultValue.GetType())
            {
                settingsObject.set(settingName, defaultValue);
            }        
        }

        private string getSettingName(QSetting setting)
        {
            int settingId = (int)setting;
            string settingName = PREFS_PREFIX;
            if (skinDependedSettings.Contains(settingId))            
                settingName += EditorGUIUtility.isProSkin ? PREFS_DARK : PREFS_LIGHT;            
            settingName += setting.ToString("G");
            return settingName.ToString();
        }
	}
}