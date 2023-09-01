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


    [SerializeField]
    private float zoomSpeed = 5f;
    [SerializeField]
    private float minZoom = 5f;
    [SerializeField]
    private float maxZoom = 20f;
    [SerializeField]
    private float smoothTime = 0.2f;

    private float targetZoom = 0f;
    private float currentZoomVelocity = 0f;

    private Camera mainCamera = null;

}
