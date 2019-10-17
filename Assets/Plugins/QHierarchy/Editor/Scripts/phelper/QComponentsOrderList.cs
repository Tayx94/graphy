using UnityEditor;
using UnityEngine;
using System;
using qtools.qhierarchy.pdata;
using System.Text;

namespace qtools.qhierarchy.phelper
{
    public class QComponentsOrderList
    {
        // PRIVATE
        private EditorWindow window;
        private Texture2D dragButton;
        private bool dragAndDrop = false;
        private float dragOffset;
        private int originalDragIndex;
        private Color backgroundColor;

        // CONSTRUCTOR
        public QComponentsOrderList (EditorWindow window)
        {            
            this.window = window;
            dragButton = QResources.getInstance().getTexture(QTexture.QDragButton);
            backgroundColor = QResources.getInstance().getColor(QColor.BackgroundDark);
        }
        
        // PUBLIC
        public void draw(Rect rect, string[] componentIds)
        {
            Event currentEvent = Event.current;

            int currentMouseIndex = Mathf.Clamp(Mathf.RoundToInt((currentEvent.mousePosition.y - dragOffset - rect.y) / 18), 0, componentIds.Length - 1);

            if (dragAndDrop && currentEvent.type == EventType.MouseUp)      
            {
                dragAndDrop = false;
                window.Repaint();

                if (currentMouseIndex != originalDragIndex)
                {
                    string newIconOrder = "";
                    for (int j = 0; j < componentIds.Length; j++)
                    {
                        if (j == currentMouseIndex) 
                        {
                            if (j > originalDragIndex)
                            {
                                newIconOrder += componentIds[j] + ";";
                                newIconOrder += componentIds[originalDragIndex] + ";";
                            }
                            else
                            {
                                newIconOrder += componentIds[originalDragIndex] + ";";
                                newIconOrder += componentIds[j] + ";";
                            }
                        }
                        else if (j != originalDragIndex) 
                        {
                            newIconOrder += componentIds[j] + ";";
                        }
                    }
                    newIconOrder = newIconOrder.TrimEnd(';');
                    QSettings.getInstance().set(QSetting.ComponentsOrder, newIconOrder);
                    componentIds = newIconOrder.Split(';');
                }
            }
            else if (dragAndDrop && currentEvent.type == EventType.MouseDrag)
            {
                window.Repaint();
            }

            for (int i = 0; i < componentIds.Length; i++)
            {
                QHierarchyComponentEnum type = (QHierarchyComponentEnum)int.Parse(componentIds[i]);
                
                Rect curRect = new Rect(rect.x, rect.y + 18 * i, rect.width, 16);
                
                if (!dragAndDrop && currentEvent.type == EventType.mouseDown && curRect.Contains(currentEvent.mousePosition))
                {
                    dragAndDrop = true;
                    originalDragIndex = i;
                    dragOffset = currentEvent.mousePosition.y - curRect.y;
                    Event.current.Use();
                }

                if (dragAndDrop)
                {
                    if (originalDragIndex != i)
                    {
                             if (i < originalDragIndex && currentMouseIndex <= i) curRect.y += 18;
                        else if (i > originalDragIndex && currentMouseIndex >= i) curRect.y -= 18;

                        drawComponentLabel(curRect, type);                
                    }
                }
                else
                {
                    drawComponentLabel(curRect, type);                    
                }
            }

            if (dragAndDrop)
            {
                float curY = currentEvent.mousePosition.y - dragOffset;
                curY = Mathf.Clamp(curY, rect.y, rect.y + rect.height - 16);
                drawComponentLabel(new Rect(rect.x, curY, rect.width, rect.height), (QHierarchyComponentEnum)int.Parse(componentIds[originalDragIndex]), true);
            }
        }
        
        // PRIVATE
        private void drawComponentLabel(Rect rect, QHierarchyComponentEnum type, bool withBackground = false)
        {
            if (withBackground)
            {
                EditorGUI.DrawRect(new Rect(rect.x, rect.y - 2, rect.width, 20), backgroundColor);
            }
            GUI.DrawTexture(new Rect(rect.x, rect.y - 2, 20, 20), dragButton);
            GUI.Label(new Rect(rect.x + 27, rect.y, rect.width - 20, 16), getTextWithSpaces(type.ToString()));
        }
        
        private string getTextWithSpaces(string text)
        {
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')                
                    newText.Append(' ');                
                newText.Append(text[i]);                
            }
            newText.Replace(" Component", "");
            return newText.ToString();
        }
    }
}

