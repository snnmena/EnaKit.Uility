using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Yoziya
{
    public class UIClickListener : UIBehaviour, IPointerClickHandler
    {
        [System.Serializable]
        public class ClickWorldEvent : UnityEvent<Vector3> { }

        [System.Serializable]
        public class ClickCanvasEvent : UnityEvent<Vector2> { }

        public ClickWorldEvent OnWorldClicked;
        public ClickCanvasEvent OnCanvasClicked;
        private RectTransform mTransform;

        protected override void Start()
        {
            mTransform = GetComponent<RectTransform>();
            //OnWorldClicked.AddListener(position => { Debug.Log(position); });
            //OnCanvasClicked.AddListener(position => { Debug.Log(position.x); });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnWorldClicked?.Invoke(eventData.position);
            Vector2 canvasPosition = ConvertToCanvasPosition(eventData.position);
            OnCanvasClicked?.Invoke(canvasPosition);
        }

        private Vector2 ConvertToCanvasPosition(Vector3 worldPosition)
        {
            Vector2 localPoint = new Vector2(
                worldPosition.x / mTransform.rect.width,
                worldPosition.y / mTransform.rect.height
            );
            return localPoint;
        }
    }
}
