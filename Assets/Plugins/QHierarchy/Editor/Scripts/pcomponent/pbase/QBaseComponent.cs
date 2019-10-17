using UnityEngine;
using System;
using System.Collections.Generic;
using qtools.qhierarchy.phierarchy;

namespace qtools.qhierarchy.pcomponent.pbase
{
    public enum QLayoutStatus
    {
        Success,
        Partly,
        Failed,
    }

    public class QBaseComponent
    {
        // PUBLIC
        public Rect rect = new Rect(0, 0, 16, 16);

        // PRIVATE
        protected bool enabled = false;
        protected bool showComponentDuringPlayMode = false;

        // CONSTRUCTOR
        public QBaseComponent()
        {
        }

        // PUBLIC
        public virtual QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            return QLayoutStatus.Success;
        }

        public virtual void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {

        }

        public virtual void eventHandler(GameObject gameObject, QObjectList objectList, Event currentEvent)
        {

        }

        public virtual void disabledHandler(GameObject gameObject, QObjectList objectList)
        {

        }

        public virtual void setEnabled(bool value)
        {
            this.enabled = value;
        }       

        public virtual bool isEnabled()
        {
            if (!enabled) 
            {
                return false;
            }
            else 
            {
                if (Application.isPlaying) return showComponentDuringPlayMode;            
                else return true;
            }
        }

        // PROTECTED
        protected void getGameObjectListRecursive(GameObject gameObject, ref List<GameObject>result, int maxDepth = int.MaxValue)
        {
            result.Add(gameObject);
            if (maxDepth > 0)
            {
                Transform transform = gameObject.transform;
                for (int i = transform.childCount - 1; i >= 0; i--)      
                    getGameObjectListRecursive(transform.GetChild(i).gameObject, ref result, maxDepth - 1);     
            }
        }
    }
}

