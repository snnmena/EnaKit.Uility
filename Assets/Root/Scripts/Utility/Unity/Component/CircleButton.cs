using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yoziya
{
    [RequireComponent(typeof(Image))]
    public class CircleButton : MonoBehaviour, ICanvasRaycastFilter
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, screenPoint, eventCamera, out localPoint);

            float halfWidth = _image.rectTransform.rect.width * 0.5f;
            float halfHeight = _image.rectTransform.rect.height * 0.5f;
            Vector2 normalizedPoint = new Vector2(localPoint.x / halfWidth, localPoint.y / halfHeight);
            Debug.Log("");
            return normalizedPoint.magnitude <= 1;
        }
    }
}