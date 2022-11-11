using UnityEngine;

namespace Graphy.Runtime.UI
{
    /// <summary>
    ///     Component that matches the size of the RectTransform to the safe area.
    /// </summary>
    [ExecuteAlways]
    public sealed class SafeArea : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private RectTransform rectTransform;
        [SerializeField] [HideInInspector] private Canvas canvas;

        [SerializeField]
        [Tooltip("If false, this will be applied only at initialization. " +
                 "If your app's screen orientation changes at runtime, set to true.")]
        private bool checkSafeAreaSizeAtRuntime;

        private bool m_isInitialized;
        private Rect m_latestSafeArea;
        private DrivenRectTransformTracker m_rectTransformTracker;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = (RectTransform)transform;
            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            if (m_isInitialized)
            {
                if (Application.isPlaying)
                {
                    // If this flag is set to false, we apply the safe area only at initialization.
                    if (!checkSafeAreaSizeAtRuntime)
                        return;

                    // If the safe area hasn't changed, do nothing.
                    if (Screen.safeArea == m_latestSafeArea)
                        return;
                }
                else
                {
                    // In Edit Mode, force update if anchorMax is zero because the RectTransform value will be incorrect if the RectTransform value is changed and undo.
                    if (rectTransform.anchorMax != Vector2.zero && Screen.safeArea == m_latestSafeArea)
                        return;
                }
            }

            // Update transform.
            SetTransform(canvas, rectTransform);

            // Make the rectTransform not editable in the inspector.
            m_rectTransformTracker.Clear();
            m_rectTransformTracker.Add(
                this,
                rectTransform,
                DrivenTransformProperties.AnchoredPosition
                | DrivenTransformProperties.SizeDelta
                | DrivenTransformProperties.AnchorMin
                | DrivenTransformProperties.AnchorMax
                | DrivenTransformProperties.Pivot
            );

            m_latestSafeArea = Screen.safeArea;
            m_isInitialized = true;
        }

        private void OnDisable()
        {
            m_rectTransformTracker.Clear();
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
