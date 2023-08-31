using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MobileJoyStick : MonoBehaviour
{

        GameObject joyStick;
        public Image joystickBackground;
        public Image joystickHandle;

        private Vector2 joystickCenter;
        private bool isJoystickActive = false;
        private Vector2 joystickInput;


        private void Start()
        {
            joystickCenter = joystickBackground.rectTransform.position;
            joyStick = GameObject.Find("VirtualJoyStick");
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (RectTransformUtility.RectangleContainsScreenPoint(joystickBackground.rectTransform, touch.position))
                        {
                            isJoystickActive = true;
                        }
                        break;

                    case TouchPhase.Moved:
                        if (isJoystickActive)
                        {
                            joystickInput = (touch.position - joystickCenter) / (joystickBackground.rectTransform.sizeDelta.x * 0.5f);
                            joystickInput = Vector2.ClampMagnitude(joystickInput, 1f);
                            joystickHandle.rectTransform.anchoredPosition = joystickInput * (joystickBackground.rectTransform.sizeDelta.x * 0.5f);
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isJoystickActive = false;
                        joystickInput = Vector2.zero;
                        joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
                        break;
                }
            }
            else
            {
                isJoystickActive = false;
                joystickInput = Vector2.zero;
                joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
            }

            if (Application.isMobilePlatform)
            {
            joyStick.SetActive(true);
            } else
            {
            joyStick.SetActive(false);
            }
        }

        public Vector2 GetJoystickInput()
        {
            return joystickInput;
        }

    
    
}
