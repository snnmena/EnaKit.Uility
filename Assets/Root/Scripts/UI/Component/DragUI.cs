using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yoziya
{
    public class DragUI : UIBehaviour, IDragHandler, IPointerDownHandler
    {
        private Vector2 offsetPos;

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position - offsetPos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            offsetPos = eventData.position - (Vector2)transform.position;
        }
    }
}
