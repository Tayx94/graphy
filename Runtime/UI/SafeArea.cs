using UnityEngine;

namespace Graphy.Runtime.UI
{
    /// <summary>
    ///     Component that matches the size of the RectTransform to the safe area.
    /// </summary>
    [ExecuteAlways]
    public sealed class SafeArea : MonoBehaviour
    {
        private Canvas m_canvas;
        private bool m_isInitialized;
        private Rect m_latestSafeArea;
        private RectTransform m_rectTransform;
        private DrivenRectTransformTracker m_rectTransformTracker;

        private RectTransform RectTransform
        {
            get
            {
                if (m_rectTransform == null)
                    m_rectTransform = (RectTransform)transform;

                return m_rectTransform;
            }
        }

        private Canvas Canvas
        {
            get
            {
                if (m_canvas == null)
                    m_canvas = GetComponentInParent<Canvas>();

                return m_canvas;
            }
        }

        private void Update()
        {
            var changed = Apply();
            if (changed)
            {
                m_rectTransformTracker.Clear();
                m_rectTransformTracker.Add(
                    this,
                    RectTransform,
                    DrivenTransformProperties.AnchoredPosition
                    | DrivenTransformProperties.SizeDelta
                    | DrivenTransformProperties.AnchorMin
                    | DrivenTransformProperties.AnchorMax
                    | DrivenTransformProperties.Pivot
                );
            }
        }

        private void OnDisable()
        {
            m_rectTransformTracker.Clear();
        }

        /// <summary>
        ///     Applies the safe area to the RectTransform.
        /// </summary>
        /// <returns>True if the RectTransform was changed.</returns>
        private bool Apply()
        {
            if (Canvas == null)
                return false;

            // Force update if anchorMax is not zero because the RectTransform value will be incorrect if the RectTransform value is changed and undo.
            if (RectTransform.anchorMax != Vector2.zero && m_isInitialized && Screen.safeArea == m_latestSafeArea)
                return false;

            SetTransform(Canvas, RectTransform);

            m_latestSafeArea = Screen.safeArea;
            m_isInitialized = true;
            return true;
        }

        private static void SetTransform(Canvas canvas, RectTransform rectTransform)
        {
            var scaleFactor = canvas.scaleFactor;
            var position = Screen.safeArea.position / scaleFactor;
            var size = Screen.safeArea.size / scaleFactor;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = size;
        }
    }
}
