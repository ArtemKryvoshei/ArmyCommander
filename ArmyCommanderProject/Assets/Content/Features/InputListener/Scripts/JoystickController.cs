using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.InputListener.Scripts
{
    public class JoystickController : MonoBehaviour
    {
        [SerializeField] private GameObject joystick;
        private IEventBus _eventBus;
        private bool _isTracking;
        private Vector2 _startPosition;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<ScreenTouchedEvent>(OnScreenTouched);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<ScreenTouchedEvent>(OnScreenTouched);
        }

        private void OnScreenTouched(ScreenTouchedEvent evt)
        {
            if (evt.Position != Vector2.zero)
            {
                StartTracking(evt.Position);
            }
            else
            {
                StopTracking();
            }
        }
        
        public void StartTracking(Vector2 startPosition)
        {
            _startPosition = startPosition;
            _isTracking = true;
            joystick.transform.position = startPosition;
            joystick.SetActive(true);

            TrackJoystick().Forget();
        }

        public void StopTracking()
        {
            _isTracking = false;
            joystick.SetActive(false);
            _eventBus.Publish(new JoystickReleasedEvent());
        }
        
        private async UniTask TrackJoystick()
        {
            while (_isTracking)
            {
#if UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    Vector2 direction = touch.position - _startPosition;
                    _eventBus.Publish(new JoystickMoveEvent(direction.normalized));
                }
#else
            Vector2 direction = (Vector2)Input.mousePosition - _startPosition;
            _eventBus.Publish(new JoystickMoveEvent(direction.normalized));
#endif
                await UniTask.Yield();
            }
        }
    }
}