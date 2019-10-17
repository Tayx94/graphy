using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pdata;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace qtools.qhierarchy.phelper
{
    public class QObjectListManager
    {
        // CONST
        private const string QObjectListName = "QHierarchyObjectList";

        // SINGLETON
        private static QObjectListManager instance;
        public static QObjectListManager getInstance()
        {
            if (instance == null) instance = new QObjectListManager();
            return instance;
        }

        // PRIVATE
        private bool showObjectList;
        private bool preventSelectionOfLockedObjects;
        private bool preventSelectionOfLockedObjectsDuringPlayMode;
        private GameObject lastSelectionGameObject = null;
        private int lastSelectionCount = 0;

        // CONSTRUCTOR
        private QObjectListManager()
        {
            QSettings.getInstance().addEventListener(QSetting.AdditionalShowHiddenQHierarchyObjectList , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LockPreventSelectionOfLockedObjects, settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LockShow              , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.LockShowDuringPlayMode, settingsChanged);
            settingsChanged();
        }

        private void settingsChanged()
        {
            showObjectList = QSettings.getInstance().get<bool>(QSetting.AdditionalShowHiddenQHierarchyObjectList);
            preventSelectionOfLockedObjects = QSettings.getInstance().get<bool>(QSetting.LockShow) && QSettings.getInstance().get<bool>(QSetting.LockPreventSelectionOfLockedObjects);
            preventSelectionOfLockedObjectsDuringPlayMode = preventSelectionOfLockedObjects && QSettings.getInstance().get<bool>(QSetting.LockShowDuringPlayMode);
        }

        private bool isSelectionChanged()
        {
            if (lastSelectionGameObject != Selection.activeGameObject || lastSelectionCount  != Selection.gameObjects.Length)
            {
                lastSelectionGameObject = Selection.activeGameObject;
                lastSelectionCount = Selection.gameObjects.Length;
                return true;
            }
            return false;
        }

        public void validate()
        {
            QObjectList.instances.RemoveAll(item => item == null);
            foreach (QObjectList objectList in QObjectList.instances)
                objectList.checkIntegrity();
            #if UNITY_5_3_OR_NEWER
            objectListDictionary.Clear();
            foreach (QObjectList objectList in QObjectList.instances)            
                objectListDictionary.Add(objectList.gameObject.scene, objectList);
            #endif
        }

        #if UNITY_5_3_OR_NEWER
        private Dictionary<Scene, QObjectList> objectListDictionary = new Dictionary<Scene, QObjectList>();
        private Scene lastActiveScene;
        private int lastSceneCount = 0;

        public void update()
        {
            try
            {     
                List<QObjectList> objectListList = QObjectList.instances;
                int objectListCount = objectListList.Count;
                if (objectListCount > 0) 
                {
                    for (int i = objectListCount - 1; i >= 0; i--)
                    {
                        QObjectList objectList = objectListList[i];
                        Scene objectListScene = objectList.gameObject.scene;
						
						if (objectListDictionary.ContainsKey(objectListScene) && objectListDictionary[objectListScene] == null)
                            objectListDictionary.Remove(objectListScene);
							
                        if (objectListDictionary.ContainsKey(objectListScene))
                        {
                            if (objectListDictionary[objectListScene] != objectList)
                            {
                                objectListDictionary[objectListScene].merge(objectList);
                                GameObject.DestroyImmediate(objectList.gameObject);
                            }
                        }
                        else
                        {
                            objectListDictionary.Add(objectListScene, objectList);
                        }
                    }

                    foreach (KeyValuePair<Scene, QObjectList> objectListKeyValue in objectListDictionary)
                    {
                        QObjectList objectList = objectListKeyValue.Value;
                        setupObjectList(objectList);
                        if (( showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy)  > 0)) ||
                            (!showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy) == 0)))
                        {
                            objectList.gameObject.hideFlags ^= HideFlags.HideInHierarchy;      
                            EditorApplication.DirtyHierarchyWindowSorting();
                        }
                    }
                    
                    if ((!Application.isPlaying && preventSelectionOfLockedObjects) || 
                        ((Application.isPlaying && preventSelectionOfLockedObjectsDuringPlayMode)) && 
                        isSelectionChanged())
                    {
                        GameObject[] selections = Selection.gameObjects;
                        List<GameObject> actual = new List<GameObject>(selections.Length);
                        bool found = false;
                        for (int i = selections.Length - 1; i >= 0; i--)
                        {
                            GameObject gameObject = selections[i];
                            
                            if (objectListDictionary.ContainsKey(gameObject.scene))
                            {
                                bool isLock = objectListDictionary[gameObject.scene].lockedObjects.Contains(selections[i]);
                                if (!isLock) actual.Add(selections[i]);
                                else found = true;
                            }
                        }
                        if (found) Selection.objects = actual.ToArray();
                    }   

                    lastActiveScene = EditorSceneManager.GetActiveScene();
                    lastSceneCount = EditorSceneManager.loadedSceneCount;
                }
            }
            catch 
            {
            }
        }

        public QObjectList getObjectList(GameObject gameObject, bool createIfNotExist = true)
        { 
            QObjectList objectList = null;
            objectListDictionary.TryGetValue(gameObject.scene, out objectList);
            
            if (objectList == null && createIfNotExist)
            {         
                objectList = createObjectList(gameObject);
                if (gameObject.scene != objectList.gameObject.scene) EditorSceneManager.MoveGameObjectToScene(objectList.gameObject, gameObject.scene);
                objectListDictionary.Add(gameObject.scene, objectList);
            }

            return objectList;
        }

        public bool isSceneChanged()
        {
            if (lastActiveScene != EditorSceneManager.GetActiveScene() || lastSceneCount != EditorSceneManager.loadedSceneCount)
                return true;
            else 
                return false;
        }

        #else

        public void update()
        {
            try
            {  
                List<QObjectList> objectListList = QObjectList.instances;
                int objectListCount = objectListList.Count;
                if (objectListCount > 0) 
                {
                    if (objectListCount > 1)
                    {
                        for (int i = objectListCount - 1; i > 0; i--)
                        {
                            objectListList[0].merge(objectListList[i]);
                            GameObject.DestroyImmediate(objectListList[i].gameObject);
                        }
                    }

                    QObjectList objectList = QObjectList.instances[0];
                    setupObjectList(objectList);

                    if (( showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy)  > 0)) ||
                        (!showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy) == 0)))
                    {
                        objectList.gameObject.hideFlags ^= HideFlags.HideInHierarchy; 
                        EditorApplication.DirtyHierarchyWindowSorting();
                    }

                    if ((!Application.isPlaying && preventSelectionOfLockedObjects) || 
                        ((Application.isPlaying && preventSelectionOfLockedObjectsDuringPlayMode))
                        && isSelectionChanged())
                    {
                        GameObject[] selections = Selection.gameObjects;
                        List<GameObject> actual = new List<GameObject>(selections.Length);
                        bool found = false;
                        for (int i = selections.Length - 1; i >= 0; i--)
                        {
                            GameObject gameObject = selections[i];
                            
                            bool isLock = objectList.lockedObjects.Contains(gameObject);                        
                            if (!isLock) actual.Add(selections[i]);
                            else found = true;
                        }
                        if (found) Selection.objects = actual.ToArray();
                    }   
                }
            }
            catch 
            {
            }
        }

        public QObjectList getObjectList(GameObject gameObject, bool createIfNotExists = false)
        { 
            List<QObjectList> objectListList = QObjectList.instances;
            int objectListCount = objectListList.Count;
            if (objectListCount != 1)
            {
                if (objectListCount == 0) 
                {
                    if (createIfNotExists)
                    {
                        createObjectList(gameObject);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
                
            return QObjectList.instances[0];
        }

        #endif

        private QObjectList createObjectList(GameObject gameObject)
        {
            GameObject gameObjectList = new GameObject();
            gameObjectList.name = QObjectListName;
            QObjectList objectList = gameObjectList.AddComponent<QObjectList>();
            setupObjectList(objectList);
            return objectList;
        }

        private void setupObjectList(QObjectList objectList)
        {
            if (objectList.tag == "EditorOnly") objectList.tag = "Untagged";
            MonoScript monoScript = MonoScript.FromMonoBehaviour(objectList);
            if (MonoImporter.GetExecutionOrder(monoScript) != -10000)                    
                MonoImporter.SetExecutionOrder(monoScript, -10000);
        }
    }
}

