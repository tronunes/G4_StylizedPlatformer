using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraPivotHorizontal;
    [SerializeField] private Transform cameraPivotVertical;
    [SerializeField] private GameObject crosshair;
    public bool inputLocked = false;

    [Header("Zooming")]
    [SerializeField] AnimationCurve zoomAnimationCurve;
    private bool isZoomed = false;
    private Vector3 normalCameraPosition = new Vector3(0f, 0f, -3f);
    private Vector3 zoomedCameraPosition = new Vector3(.6f, .3f, -1.8f);
    private float normalCameraFov = 60f;
    private float zoomedCameraFov = 45f;
    private float zoomTransitionTime = 0.15f;
    private float currentTransitionTime = 0f;
    private bool isZoomingIn = false;
    private bool isZoomingOut = false;


    [Header("Settings")]
    private float horizontalRotateSpeedSetting = 3f;
    private float verticalRotateSpeedSetting = 3f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Don't control the Camera while paused
        if (GameManager.instance.IsPaused() || inputLocked)
        {
            return;
        }

        // Case: Start zooming in
        if (!isZoomed && !isZoomingIn && Input.GetAxisRaw("Fire2") > 0f && currentTransitionTime <= 0f)
        {
            isZoomed = true;
            isZoomingIn = true;
            currentTransitionTime = zoomTransitionTime;
        }
        // Case: Start zooming out
        else if (isZoomed && !isZoomingOut && Input.GetAxisRaw("Fire2") <= 0f && currentTransitionTime <= 0f)
        {
            isZoomed = false;
            isZoomingOut = true;
            currentTransitionTime = zoomTransitionTime;
        }

        // Reduce zoom transition timer
        currentTransitionTime -= Time.deltaTime;

        // Case: Zooming in
        if (isZoomingIn)
        {
            cameraTransform.localPosition = Vector3.Lerp(
                normalCameraPosition,
                zoomedCameraPosition,
                zoomAnimationCurve.Evaluate(1f - currentTransitionTime / zoomTransitionTime)
            );

            cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(
                normalCameraFov,
                zoomedCameraFov,
                zoomAnimationCurve.Evaluate(1f - currentTransitionTime / zoomTransitionTime)
            );

            // Fully zoomed in
            if (currentTransitionTime <= 0f)
            {
                currentTransitionTime = 0f;
                isZoomingIn = false;
            }
        }
        // Case: Zooming out
        else if (isZoomingOut)
        {
            cameraTransform.localPosition = Vector3.Lerp(
                normalCameraPosition,
                zoomedCameraPosition,
                zoomAnimationCurve.Evaluate(currentTransitionTime / zoomTransitionTime)
            );

            cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(
                normalCameraFov,
                zoomedCameraFov,
                zoomAnimationCurve.Evaluate(currentTransitionTime / zoomTransitionTime)
            );

            // Fully zoomed out
            if (currentTransitionTime <= 0f)
            {
                currentTransitionTime = 0f;
                isZoomingOut = false;
            }
        }

        // Toggle crosshair visibility
        crosshair.SetActive(isZoomed);

        // Let PlayerMovement know if zoomed or not
        gameObject.GetComponent<PlayerMovement>().SetIsZoomed(isZoomed);

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

    public bool IsZoomed()
    {
        return isZoomed;
    }

    public void ResetPlayerCamera()
    {
        cameraTransform.localPosition = normalCameraPosition;
        cameraPivotHorizontal.localRotation = Quaternion.identity;
        cameraPivotVertical.localRotation = Quaternion.Euler(25f, 0f, 0f);

        crosshair.SetActive(false);
        isZoomed = false;
        cameraTransform.GetComponent<Camera>().fieldOfView = normalCameraFov;
        currentTransitionTime = 0f;
        isZoomingIn = false;
        isZoomingOut = false;
    }
}
