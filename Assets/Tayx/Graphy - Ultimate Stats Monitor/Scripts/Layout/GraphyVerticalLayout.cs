using UnityEngine;

namespace Tayx.Graphy.Layout
{
    public class GraphyVerticalLayout : MonoBehaviour
    {
        private enum LayoutPosition { LEFT_BOTTOM, LEFT_TOP, RIGHT_BOTTOM, RIGHT_TOP };

        [SerializeField] private LayoutPosition layoutPosition = LayoutPosition.LEFT_BOTTOM;
        [SerializeField] private float padding = 0f;
        [SerializeField] private float spacing = 0f;

        private Vector2 modulePivot = Vector2.zero;
        private Vector3 targetPosition;

        private void Start()
        {
            Init();
            UpdateModuleLayout();
        }

        public void UpdateModuleLayout()
        {
            targetPosition.x = Mathf.Approximately(modulePivot.x, 0f) ? padding : -padding;
            targetPosition.y = Mathf.Approximately(modulePivot.y, 0f) ? padding : -padding;

            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();

                if (rect != null)
                {
                    rect.pivot = modulePivot;
                    rect.localPosition = targetPosition;

                    targetPosition.y += ((rect.sizeDelta.y * rect.localScale.y) + spacing) * (Mathf.Approximately(modulePivot.y, 0f) ? 1f : -1f);
                }
            }
        }

        private void Init()
        {
            modulePivot.x = layoutPosition == LayoutPosition.LEFT_BOTTOM || layoutPosition == LayoutPosition.LEFT_TOP ? 0f : 1f;
            modulePivot.y = layoutPosition == LayoutPosition.LEFT_BOTTOM || layoutPosition == LayoutPosition.RIGHT_BOTTOM ? 0f : 1f;
        }
    }
}
