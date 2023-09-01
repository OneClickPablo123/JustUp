using UnityEngine;
using UnityEngine.UI;

public class MobileJoyStick : MonoBehaviour
{
    GameObject joyStick;
    public Image joystickBackground;
    public Image joystickHandle;

    private Vector2 joystickCenter;
    private bool isJoystickActive = false;
    private float joystickInput;

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            joystickCenter = joystickBackground.rectTransform.position;
            joyStick = GameObject.Find("VirtualJoyStick");
        }
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
                        float rawX = (touch.position.x - joystickCenter.x) / (joystickBackground.rectTransform.sizeDelta.x * 0.5f);
                        joystickInput = Mathf.Clamp(rawX, -1f, 1f);
                        joystickHandle.rectTransform.anchoredPosition = new Vector2(joystickInput * (joystickBackground.rectTransform.sizeDelta.x * 0.5f), 0f);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isJoystickActive = false;
                    joystickInput = 0f;
                    joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
                    break;
            }
        }
        else
        {
            isJoystickActive = false;
            joystickInput = 0f;
            joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public float GetMappedJoystickInput()
    {
        // Map the joystickInput to the range 0.1 - 1
        float mappedInput = Mathf.Lerp(-1, 1f, (joystickInput + 1f) / 2f);
        return mappedInput;
    }
}