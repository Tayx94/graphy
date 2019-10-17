using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.phierarchy;
using qtools.qhierarchy.phelper;
using qtools.qhierarchy.pdata;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;
using System.Text;

namespace qtools.qhierarchy.pcomponent
{
    public class QErrorComponent: QBaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Texture2D errorIconTexture;
        private bool showErrorOfChildren;
        private bool showErrorTypeReferenceIsNull;
        private bool showErrorTypeReferenceIsMissing;
        private bool showErrorTypeStringIsEmpty;
        private bool showErrorIconScriptIsMissing;
        private bool showErrorIconWhenTagIsUndefined;
        private bool showErrorForDisabledComponents;
        private bool showErrorIconMissingEventMethod;
        private bool showErrorForDisabledGameObjects;
        private List<string> ignoreErrorOfMonoBehaviours;
        private StringBuilder errorStringBuilder;
        private int errorCount;

        // CONSTRUCTOR
        public QErrorComponent ()
        {
            rect.width = 7; 

            errorIconTexture = QResources.getInstance().getTexture(QTexture.QErrorIcon);

            QSettings.getInstance().addEventListener(QSetting.ErrorShowIconOnParent             , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowReferenceIsNull          , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowReferenceIsMissing       , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowStringIsEmpty            , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowScriptIsMissing          , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowForDisabledComponents    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowForDisabledGameObjects   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowMissingEventMethod       , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowWhenTagOrLayerIsUndefined, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShow                         , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorShowDuringPlayMode           , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.ErrorIgnoreString                 , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalActiveColor             , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.AdditionalInactiveColor           , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            showErrorOfChildren             = QSettings.getInstance().get<bool>(QSetting.ErrorShowIconOnParent);
            showErrorTypeReferenceIsNull    = QSettings.getInstance().get<bool>(QSetting.ErrorShowReferenceIsNull);
            showErrorTypeReferenceIsMissing = QSettings.getInstance().get<bool>(QSetting.ErrorShowReferenceIsMissing);
            showErrorTypeStringIsEmpty      = QSettings.getInstance().get<bool>(QSetting.ErrorShowStringIsEmpty);
            showErrorIconScriptIsMissing    = QSettings.getInstance().get<bool>(QSetting.ErrorShowScriptIsMissing);
            showErrorForDisabledComponents  = QSettings.getInstance().get<bool>(QSetting.ErrorShowForDisabledComponents);
            showErrorForDisabledGameObjects = QSettings.getInstance().get<bool>(QSetting.ErrorShowForDisabledGameObjects);
            showErrorIconMissingEventMethod = QSettings.getInstance().get<bool>(QSetting.ErrorShowMissingEventMethod);
            showErrorIconWhenTagIsUndefined = QSettings.getInstance().get<bool>(QSetting.ErrorShowWhenTagOrLayerIsUndefined);
            activeColor                     = QSettings.getInstance().getColor(QSetting.AdditionalActiveColor);
            inactiveColor                   = QSettings.getInstance().getColor(QSetting.AdditionalInactiveColor);
            enabled                         = QSettings.getInstance().get<bool>(QSetting.ErrorShow);
            showComponentDuringPlayMode     = QSettings.getInstance().get<bool>(QSetting.ErrorShowDuringPlayMode);

            string ignoreErrorOfMonoBehavioursString = QSettings.getInstance().get<string>(QSetting.ErrorIgnoreString);
            if (ignoreErrorOfMonoBehavioursString != "") 
            {
                ignoreErrorOfMonoBehaviours = new List<string>(ignoreErrorOfMonoBehavioursString.Split(new char[] { ',', ';', '.', ' ' }));
                ignoreErrorOfMonoBehaviours.RemoveAll(item => item == "");
            }
            else ignoreErrorOfMonoBehaviours = null;
        }

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 7) 
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 7;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {
            bool errorFound = findError(gameObject, gameObject.GetComponents<MonoBehaviour>());

            if (errorFound)
            {           
                QColorUtils.setColor(activeColor);
                GUI.DrawTexture(rect, errorIconTexture);
                QColorUtils.clearColor();
            }
            else if (showErrorOfChildren) 
            {
                errorFound = findError(gameObject, gameObject.GetComponentsInChildren<MonoBehaviour>(true));
                if (errorFound) 
                {
                    QColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, errorIconTexture);
                    QColorUtils.clearColor();
                }
            }            
        }

        public override void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();

                errorCount = 0;
                errorStringBuilder = new StringBuilder();
                findError(gameObject, gameObject.GetComponents<MonoBehaviour>(), true);

                if (errorCount > 0)
                {
                    EditorUtility.DisplayDialog(errorCount + (errorCount == 1 ? " error was found" : " errors were found"), errorStringBuilder.ToString(), "OK");
                }
            }
        }

        // PRIVATE
        private bool findError(GameObject gameObject, MonoBehaviour[] components, bool printError = false)
        {
            if (showErrorIconWhenTagIsUndefined)
            {
                try
                { 
                    gameObject.tag.CompareTo(null); 
                }
                catch 
                {
                    if (printError)
                    {
                        appendErrorLine("Tag is undefined");
                    }
                    else
                    {
                        return true;
                    }
                }

                if (LayerMask.LayerToName(gameObject.layer).Equals("")) 
                {
                    if (printError)
                    {
                        appendErrorLine("Layer is undefined");
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            for (int i = 0; i < components.Length; i++)
            {
                MonoBehaviour monoBehaviour = components[i];
                if (monoBehaviour == null)
                {
                    if (showErrorIconScriptIsMissing)
                    {
                        if (printError)
                        {
                            appendErrorLine("Component #" + i + " is missing");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (ignoreErrorOfMonoBehaviours != null)
                    {
                        for (int j = ignoreErrorOfMonoBehaviours.Count - 1; j >= 0; j--)
                        {
                            if (monoBehaviour.GetType().FullName.Contains(ignoreErrorOfMonoBehaviours[j]))
                            {
                                return false;
                            } 
                        }
                    }

                    if (showErrorIconMissingEventMethod)
                    {
                        if (monoBehaviour.gameObject.activeSelf || showErrorForDisabledComponents)
                        {
                            try
                            {
                                if (isUnityEventsNullOrMissing(monoBehaviour, printError))
                                {
                                    if (!printError)
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (showErrorTypeReferenceIsNull || showErrorTypeStringIsEmpty || showErrorTypeReferenceIsMissing)
                    {                       
                        if (!monoBehaviour.enabled && !showErrorForDisabledComponents) continue;
                        if (!monoBehaviour.gameObject.activeSelf && !showErrorForDisabledGameObjects) continue;

                        BindingFlags bf = BindingFlags.Instance | BindingFlags.Public;
                        if (!monoBehaviour.GetType().FullName.Contains("UnityEngine")) bf |= BindingFlags.NonPublic;
                        FieldInfo[] fieldArray = monoBehaviour.GetType().GetFields(bf);

                        for (int j = 0; j < fieldArray.Length; j++)
                        {
                            FieldInfo field = fieldArray[j];

                            if (System.Attribute.IsDefined(field, typeof(HideInInspector)) || field.IsStatic) continue;     

                            if (field.IsPrivate || !field.IsPublic) 
                            {
                                if (!Attribute.IsDefined(field, typeof(SerializeField)))
                                {
                                    continue;
                                }
                            }

                            object value = field.GetValue(monoBehaviour);

                            try
                            {
                                if (showErrorTypeStringIsEmpty && field.FieldType == typeof(string) && value != null && ((string)value).Equals(""))
                                {                                
                                    if (printError)
                                    {
                                        appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name + ": String value is empty");
                                        continue;
                                    }
                                    else
                                    {
                                        return true;                                 
                                    }
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (showErrorTypeReferenceIsMissing && value != null && value is Component && (Component)value == null)
                                {
                                    if (printError)
                                    {
                                        appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name + ": Reference is missing");
                                        continue;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (showErrorTypeReferenceIsNull && (value == null || value.Equals(null)))
                                {           
                                    if (printError)
                                    {
                                        appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name + ": Reference is null");
                                        continue;
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                            }
                            catch
                            {
                            }
                                      
                            try
                            {
                                if (showErrorTypeReferenceIsNull && value != null && (value is IEnumerable))
                                {
                                    foreach (var item in (IEnumerable)value)
                                    {
                                        if (item == null || item.Equals(null))
                                        {
                                            if (printError)
                                            {
                                                appendErrorLine(monoBehaviour.GetType().Name + "." + field.Name + ": IEnumerable has value with null reference");
                                                continue;
                                            }
                                            else
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }                            
                        }
                    }
                }
            }
            return false;
        }

        private List<string> targetPropertiesNames = new List<string>(10);
        
        private bool isUnityEventsNullOrMissing(MonoBehaviour monoBehaviour, bool printError) 
        {
            targetPropertiesNames.Clear();
            FieldInfo[] fieldArray = monoBehaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); 
   
            for (int i = fieldArray.Length - 1; i >= 0; i--) 
            {
                FieldInfo field = fieldArray[i];                    
                if (field.FieldType == typeof(UnityEventBase) || field.FieldType.IsSubclassOf(typeof(UnityEventBase))) 
                {
                    targetPropertiesNames.Add(field.Name);
                }
            }
            
            if (targetPropertiesNames.Count > 0) 
            {
                SerializedObject serializedMonoBehaviour = new SerializedObject(monoBehaviour); 
                for (int i = targetPropertiesNames.Count - 1; i >= 0; i--) 
                {
                    string targetProperty = targetPropertiesNames[i];

                    SerializedProperty property = serializedMonoBehaviour.FindProperty(targetProperty);
                    SerializedProperty propertyRelativeArrray = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
                    
                    for (int j = propertyRelativeArrray.arraySize - 1; j >= 0; j--)
                    {
                        SerializedProperty arrayElementAtIndex = propertyRelativeArrray.GetArrayElementAtIndex(j);

                        SerializedProperty propertyTarget       = arrayElementAtIndex.FindPropertyRelative("m_Target");
                        if (propertyTarget.objectReferenceValue == null)
                        {
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name + ": Event object reference is null");
                            }
                            else
                            {
                                return true;
                            }
                        }

                        SerializedProperty propertyMethodName   = arrayElementAtIndex.FindPropertyRelative("m_MethodName");
                        if (string.IsNullOrEmpty(propertyMethodName.stringValue)) 
                        {
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name + ": Event handler function is not selected");
                                continue;
                            }
                            else
                            {
                                return true;
                            }
                        }
                         
                        string argumentAssemblyTypeName = arrayElementAtIndex.FindPropertyRelative("m_Arguments").FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue;
                        System.Type argumentAssemblyType;
                        if (!string.IsNullOrEmpty(argumentAssemblyTypeName)) argumentAssemblyType = System.Type.GetType(argumentAssemblyTypeName, false) ?? typeof(UnityEngine.Object);
                        else argumentAssemblyType = typeof(UnityEngine.Object);

                        UnityEventBase dummyEvent;
                        System.Type propertyTypeName = System.Type.GetType(property.FindPropertyRelative("m_TypeName").stringValue, false);
                        if (propertyTypeName == null) dummyEvent = (UnityEventBase) new UnityEvent();
                        else dummyEvent = Activator.CreateInstance(propertyTypeName) as UnityEventBase;

                        if (!UnityEventDrawer.IsPersistantListenerValid(dummyEvent, propertyMethodName.stringValue, propertyTarget.objectReferenceValue, (PersistentListenerMode)arrayElementAtIndex.FindPropertyRelative("m_Mode").enumValueIndex, argumentAssemblyType))
                        { 
                            if (printError)
                            {
                                appendErrorLine(monoBehaviour.GetType().Name + ": Event handler function is missing");
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }   
            }            
            return false;
        }

        private void appendErrorLine(string error)
        {
            errorCount++;
            errorStringBuilder.Append(errorCount.ToString());
            errorStringBuilder.Append(") ");
            errorStringBuilder.AppendLine(error);
        }
    }
}

