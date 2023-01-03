using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/BetterButton", 30)]
    public class BetterButton : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, IPointerDownHandler, IPointerUpHandler 
    {
        protected BetterButton(){}

        public bool buttonPressed;

        public ButtonClickedEvent onClick;

        public virtual void OnPointerClick(PointerEventData eventData){}
        public virtual void OnSubmit(BaseEventData eventData){}

        public override void OnPointerDown(PointerEventData eventData)
        {
            buttonPressed = true;
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            buttonPressed = false;
        }       

        public class ButtonClickedEvent : UnityEvent
        {
            public ButtonClickedEvent(){}
        }
    }
}
