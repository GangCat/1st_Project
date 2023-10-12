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

    public void WarpCameraWithPos(Vector3 _pos)
    {
        // transform.position = _pos + cameraOffset;
        
        Vector3 newPos = _pos + cameraOffset;
        
        // 카메라 위치 제한
        transform.position = ClampCameraPosition(newPos);
        
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
        
        // 카메라 위치 제한
        transform.position = ClampCameraPosition(Vector3.Lerp(transform.position, screenMovePos, Time.deltaTime));
    }

    public void MoveCameraWithKey(Vector2 _arrowKeyInput)
    {
        screenMovePos = transform.position;

        screenMovePos += transform.right * _arrowKeyInput.x * camMoveSpeed * Time.deltaTime;
        screenMovePos += Quaternion.Euler(0f, 45f, 0f) * Vector3.forward * _arrowKeyInput.y * camMoveSpeed * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, screenMovePos, Time.deltaTime);
        
        // 카메라 위치 제한
        transform.position = ClampCameraPosition(Vector3.Lerp(transform.position, screenMovePos, Time.deltaTime));
    }

    public void MoveCameraWithObject(Vector3 _objectPos)
    {
        // transform.position = _objectPos + cameraOffset;
        
        Vector3 newPos = _objectPos + cameraOffset;
        
        // 카메라 위치 제한
        transform.position = ClampCameraPosition(newPos);
        
    }
    
    
    // 카메라 이동 범위 제한
    private Vector3 ClampCameraPosition(Vector3 _targetPos)
    {
        // 카메라의 현재 orthographicSize에 따라 제한 범위를 계산
        float aspectRatio = mainCamera.aspect;  // 카메라의 가로/세로 비율
        float cameraHeight = mainCamera.orthographicSize;  // 카메라의 높이
        float cameraWidth = cameraHeight * aspectRatio;  // 카메라의 너비

        // 현재 카메라 크기를 고려하여 실제 제한 범위를 계산
        float actualMinX = minX + cameraWidth;
        float actualMaxX = maxX - cameraWidth;
        float actualMinZ = minZ + cameraHeight;
        float actualMaxZ = maxZ - cameraHeight;

        // 45도 회전시켜 가상의 위치를 계산합니다.
        float rotatedX = _targetPos.x * Mathf.Cos(Mathf.Deg2Rad * 45) - _targetPos.z * Mathf.Sin(Mathf.Deg2Rad * 45);
        float rotatedZ = _targetPos.x * Mathf.Sin(Mathf.Deg2Rad * 45) + _targetPos.z * Mathf.Cos(Mathf.Deg2Rad * 45);
    
        // 가상의 위치를 제한합니다.
        float clampedX = Mathf.Clamp(rotatedX, actualMinX, actualMaxX);
        float clampedZ = Mathf.Clamp(rotatedZ, actualMinZ, actualMaxZ);
    
        // 제한된 가상의 위치를 다시 역회전시켜 실제 위치를 얻습니다.
        float unrotatedX = clampedX * Mathf.Cos(Mathf.Deg2Rad * -45) - clampedZ * Mathf.Sin(Mathf.Deg2Rad * -45);
        float unrotatedZ = clampedX * Mathf.Sin(Mathf.Deg2Rad * -45) + clampedZ * Mathf.Cos(Mathf.Deg2Rad * -45);
    
        return new Vector3(unrotatedX, _targetPos.y, unrotatedZ);
    }

    
    
    [SerializeField]
    private Vector3 cameraOffset = Vector3.zero;
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
    
    [SerializeField]
    private float minX, maxX, minZ, maxZ;  // 카메라 이동 제한 최대 좌표

    private float targetZoom = 0f;
    private float currentZoomVelocity = 0f;

    private Vector3 screenMovePos = Vector3.zero;

    private Camera mainCamera = null;

}
