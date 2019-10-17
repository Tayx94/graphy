using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using qtools.qhierarchy.pdata;
using qtools.qhierarchy.phierarchy;
using UnityEditor.Callbacks;
using qtools.qhierarchy.phelper;

namespace qtools.qhierarchy
{
    [InitializeOnLoad]
    public class QHierarchyInitializer
    {
        private static QHierarchy hierarchy;

        static QHierarchyInitializer()
        {
            EditorApplication.update -= update;
            EditorApplication.update += update;

            EditorApplication.hierarchyWindowItemOnGUI -= hierarchyWindowItemOnGUIHandler;
            EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowItemOnGUIHandler;
            
            EditorApplication.hierarchyWindowChanged   -= hierarchyWindowChanged;
            EditorApplication.hierarchyWindowChanged   += hierarchyWindowChanged;

            Undo.undoRedoPerformed -= undoRedoPerformed;
            Undo.undoRedoPerformed += undoRedoPerformed;
        }

        static void undoRedoPerformed()
        {
            EditorApplication.RepaintHierarchyWindow();          
        }

        static void init()
        {       
            hierarchy = new QHierarchy();
        } 

        static void update()
        {
            if (hierarchy == null) init();
            QObjectListManager.getInstance().update();
        }

        static void hierarchyWindowItemOnGUIHandler(int instanceId, Rect selectionRect)
        {
            if (hierarchy == null) init();
             hierarchy.hierarchyWindowItemOnGUIHandler(instanceId, selectionRect);
        }

        static void hierarchyWindowChanged()
        {
            if (hierarchy == null) init();
            QObjectListManager.getInstance().validate();
        }
    }
}

