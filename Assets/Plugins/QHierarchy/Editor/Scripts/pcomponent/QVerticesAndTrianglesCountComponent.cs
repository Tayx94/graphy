using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using qtools.qhierarchy.pcomponent.pbase;
using qtools.qhierarchy.pdata;

namespace qtools.qhierarchy.pcomponent
{
    public class QVerticesAndTrianglesCountComponent: QBaseComponent
    {
        // PRIVATE
        private GUIStyle labelStyle;
        private Color verticesLabelColor;
        private Color trianglesLabelColor;
        private bool calculateTotalCount;
        private bool showTrianglesCount;
        private bool showVerticesCount;

        // CONSTRUCTOR
        public QVerticesAndTrianglesCountComponent ()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 8;
            labelStyle.clipping = TextClipping.Clip;  
            labelStyle.alignment = TextAnchor.MiddleRight;

            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesShow                  , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesShowDuringPlayMode    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesCalculateTotalCount   , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesShowTriangles         , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesShowVertices          , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesLabelSize             , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesVerticesLabelColor    , settingsChanged);
            QSettings.getInstance().addEventListener(QSetting.VerticesAndTrianglesTrianglesLabelColor   , settingsChanged);

            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShow);
            showComponentDuringPlayMode = QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowDuringPlayMode);
            calculateTotalCount         = QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesCalculateTotalCount);
            showTrianglesCount          = QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowTriangles);
            showVerticesCount           = QSettings.getInstance().get<bool>(QSetting.VerticesAndTrianglesShowVertices);
            verticesLabelColor          = QSettings.getInstance().getColor(QSetting.VerticesAndTrianglesVerticesLabelColor);
            trianglesLabelColor         = QSettings.getInstance().getColor(QSetting.VerticesAndTrianglesTrianglesLabelColor);
            QHierarchySize labelSize = (QHierarchySize)QSettings.getInstance().get<int>(QSetting.VerticesAndTrianglesLabelSize);
            labelStyle.fontSize = labelSize == QHierarchySize.Big ? 9 : 8;
            rect.width = labelSize == QHierarchySize.Big ? 33 : 25;
        }   

        // DRAW
        public override QLayoutStatus layout(GameObject gameObject, QObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width)
            {
                return QLayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return QLayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, QObjectList objectList, Rect selectionRect)
        {  
            int vertexCount = 0;
            int triangleCount = 0;

            MeshFilter[] meshFilterArray = calculateTotalCount ? gameObject.GetComponentsInChildren<MeshFilter>(true) : gameObject.GetComponents<MeshFilter>();
            for (int i = 0; i < meshFilterArray.Length; i++)
            {
                Mesh sharedMesh = meshFilterArray[i].sharedMesh;
                if (sharedMesh != null)
                {
                    if (showVerticesCount) vertexCount += sharedMesh.vertexCount;
                    if (showTrianglesCount) triangleCount += sharedMesh.triangles.Length;
                }
            }

            SkinnedMeshRenderer[] skinnedMeshRendererArray = calculateTotalCount ? gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true) : gameObject.GetComponents<SkinnedMeshRenderer>();
            for (int i = 0; i < skinnedMeshRendererArray.Length; i++)
            {
                Mesh sharedMesh = skinnedMeshRendererArray[i].sharedMesh;
                if (sharedMesh != null)
                {   
                    if (showVerticesCount) vertexCount += sharedMesh.vertexCount;
                    if (showTrianglesCount) triangleCount += sharedMesh.triangles.Length;
                }
            }

            triangleCount /= 3;

            if (vertexCount > 0 || triangleCount > 0)
            {
                if (showTrianglesCount && showVerticesCount) 
                {
                    rect.y -= 4;
                    labelStyle.normal.textColor = verticesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(vertexCount), labelStyle);

                    rect.y += 8;
                    labelStyle.normal.textColor = trianglesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(triangleCount), labelStyle);
                }
                else if (showVerticesCount)
                {
                    labelStyle.normal.textColor = verticesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(vertexCount), labelStyle);
                }
                else
                {
                    labelStyle.normal.textColor = trianglesLabelColor;
                    EditorGUI.LabelField(rect, getCountString(triangleCount), labelStyle);
                }
            }
        }

        // PRIVATE
        private string getCountString(int count)
        {
            if (count < 1000) return count.ToString();
            else if (count < 1000000) 
            {
                if (count > 100000) return (count / 1000.0f).ToString("0") + "k";
                else return (count / 1000.0f).ToString("0.0") + "k";
            }
            else 
            {
                if (count > 10000000) return (count / 1000.0f).ToString("0") + "M";
                else return (count / 1000000.0f).ToString("0.0") + "M";
            }
        }
    }
}

