using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraPivotHorizontal;
    [SerializeField] private Transform cameraPivotVertical;

    [Header("Settings")]
    private float horizontalRotateSpeedSetting = 3f;
    private float verticalRotateSpeedSetting = 3f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        UnZoom();
    }

    void Update()
    {
        // Don't control the Camera while paused
        if (GameManager.instance.IsPaused())
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Zoom();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            UnZoom();
        }

        // Mouse delta movement
        float pitch = Input.GetAxis("Mouse Y");
        float yaw = Input.GetAxis("Mouse X");

        cameraPivotVertical.Rotate(-pitch * verticalRotateSpeedSetting, 0f, 0f);
        cameraPivotHorizontal.Rotate(0f, yaw * horizontalRotateSpeedSetting, 0f);

        // Clamp pitch to 280 (which is in basically -80) and 80 degrees
        // Make sure other axes are always zero
        Vector3 currentEulerAngles = cameraPivotVertical.localRotation.eulerAngles;
        currentEulerAngles.y = 0f;
        currentEulerAngles.z = 0f;
        if (currentEulerAngles.x > 80f && currentEulerAngles.x < 180f)
        {
            currentEulerAngles.x = 80f;
        }
        else if (currentEulerAngles.x > 180f && currentEulerAngles.x < 280f)
        {
            currentEulerAngles.x = 280f;
        }
        cameraPivotVertical.localRotation = Quaternion.Euler(currentEulerAngles);
    }

    void Zoom()
    {
        cameraTransform.localPosition = new Vector3(.6f, .3f, -1.8f);
        cameraTransform.GetComponent<Camera>().fieldOfView = 50f;
    }

    void UnZoom()
    {
        cameraTransform.localPosition = new Vector3(0f, 0f, -3f);
        cameraTransform.GetComponent<Camera>().fieldOfView = 60f;
    }
}
