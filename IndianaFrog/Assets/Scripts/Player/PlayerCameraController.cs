using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform cameraPivotHorizontal;
    [SerializeField] private Transform cameraPivotVertical;

    [Header("Settings")]
    private float horizontalRotateSpeedSetting = 3f;
    private float verticalRotateSpeedSetting = 3f;
    private float horizontalDeadZoneSetting = 50f;
    private float verticalDeadZoneSetting = 30f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse position's offset from the center of the screen
        float mouseHorizontalOffset = Input.mousePosition.x - Screen.width / 2f;
        float mouseVerticalOffset = Input.mousePosition.y - Screen.height / 2f;

        // Adjust the camera to turn faster the more there is offset
        float directionSign = mouseHorizontalOffset > 0 ? 1f : -1f;
        float turnSpeedHorizontal = horizontalRotateSpeedSetting * ((mouseHorizontalOffset - directionSign * horizontalDeadZoneSetting) / Screen.width);
        directionSign = mouseVerticalOffset > 0 ? 1f : -1f;
        float turnSpeedVertical = verticalRotateSpeedSetting * ((mouseVerticalOffset - directionSign * verticalDeadZoneSetting) / Screen.height);

        // Horizontal rotation
        if (mouseHorizontalOffset < -horizontalDeadZoneSetting)
        {
            cameraPivotHorizontal.Rotate(0f, turnSpeedHorizontal, 0f);
        }
        if (mouseHorizontalOffset > horizontalDeadZoneSetting)
        {
            cameraPivotHorizontal.Rotate(0f, turnSpeedHorizontal, 0f);
        }

        // Vertical rotation
        if (mouseVerticalOffset < -verticalDeadZoneSetting)
        {
            cameraPivotVertical.Rotate(turnSpeedVertical, 0f, 0f);
        }
        if (mouseVerticalOffset > verticalDeadZoneSetting)
        {
            cameraPivotVertical.Rotate(turnSpeedVertical, 0f, 0f);
        }
    }
}
