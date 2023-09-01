using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public void Init()
    {
        mainCamera = GetComponent<Camera>();
        targetZoom = mainCamera.orthographicSize;
    }

    public void ZoomCamera(float _zoomRatio)
    {
        targetZoom -= _zoomRatio * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetZoom, ref currentZoomVelocity, smoothTime);
    }

    public void MoveCameraWithMouse(Vector2 _mousePos)
    {
        screenMovePos = transform.position;

        
        if (_mousePos.x < screenOffsetX)
            screenMovePos -= transform.right * camMoveSpeed * Time.deltaTime;
        else if (_mousePos.x > Screen.width - screenOffsetX)
            screenMovePos += transform.right * camMoveSpeed * Time.deltaTime;

        if (_mousePos.y < screenOffsetY)
            screenMovePos -= Quaternion.Euler(0f, 45f, 0f) * Vector3.forward * camMoveSpeed * Time.deltaTime;
        else if (_mousePos.y > Screen.height - screenOffsetY)
            screenMovePos += Quaternion.Euler(0f, 45f, 0f) * Vector3.forward * camMoveSpeed * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, screenMovePos, Time.deltaTime);
    }

    public void MoveCemeraWithKey(Vector2 _arrowKeyInput)
    {
        screenMovePos = transform.position;

        screenMovePos += transform.right * _arrowKeyInput.x * camMoveSpeed * Time.deltaTime;
        screenMovePos += Quaternion.Euler(0f, 45f, 0f) * Vector3.forward * _arrowKeyInput.y * camMoveSpeed * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, screenMovePos, Time.deltaTime);
    }


    [SerializeField]
    private float zoomSpeed = 5f;
    [SerializeField]
    private float camMoveSpeed = 20f;
    [SerializeField]
    private float minZoom = 5f;
    [SerializeField]
    private float maxZoom = 20f;
    [SerializeField]
    private float smoothTime = 0.2f;
    [SerializeField]
    private float screenOffsetX = 20f;
    [SerializeField]
    private float screenOffsetY = 10f;

    private float targetZoom = 0f;
    private float currentZoomVelocity = 0f;

    private Vector3 screenMovePos = Vector3.zero;

    private Camera mainCamera = null;

}
