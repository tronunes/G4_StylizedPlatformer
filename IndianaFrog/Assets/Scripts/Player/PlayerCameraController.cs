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
        // Mouse delta movement
        float pitch = Input.GetAxis("Mouse Y");
        float yaw = Input.GetAxis("Mouse X");

        cameraPivotVertical.Rotate(-pitch * verticalRotateSpeedSetting, 0f, 0f);
        cameraPivotHorizontal.Rotate(0f, yaw * horizontalRotateSpeedSetting, 0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        return angle;
    }
}
