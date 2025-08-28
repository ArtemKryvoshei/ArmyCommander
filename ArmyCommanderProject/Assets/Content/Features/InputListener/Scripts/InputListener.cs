using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Content.Features.InputListener.Scripts
{
    public class InputListener : MonoBehaviour
    {
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && touch.phase == TouchPhase.Began)
                {
                    _eventBus.Publish(new ScreenTouchedEvent(touch.position));
                }
            }
            else
            {
                _eventBus.Publish(new ScreenTouchedEvent(Vector2.zero));
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _eventBus.Publish(new ScreenTouchedEvent(Input.mousePosition));
            }
#endif
        }
    }
}