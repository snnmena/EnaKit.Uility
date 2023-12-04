using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yoziya
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public RectTransform joystickBackground;
        public RectTransform joystickHandle;

        private Vector2 inputDirection;

        public delegate void JoystickEventHandler(Vector2 direction);

        public event JoystickEventHandler JoystickMoved;
        public event Action JoystickReleased;


        private void Start()
        {
            //JoystickMoved += position => { Debug.Log(position); };
            //JoystickReleased += () => Debug.Log("stop");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - (Vector2)joystickBackground.position;
            inputDirection = direction.magnitude < joystickBackground.sizeDelta.x / 2 ? direction / (joystickBackground.sizeDelta.x / 2) : direction.normalized;
            joystickHandle.anchoredPosition = inputDirection * (joystickBackground.sizeDelta.x / 2);
            JoystickMoved?.Invoke(inputDirection); // 触发事件
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputDirection = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
            JoystickReleased?.Invoke(); // 触发事件
        }
    }
}
