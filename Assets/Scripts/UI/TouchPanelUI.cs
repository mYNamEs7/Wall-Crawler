using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TouchPanelUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static event Action OnFingerUp;
        public static bool IsPressed { get; private set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            OnFingerUp?.Invoke();
        }
    }
}
