using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init()
    {
        cameraMove = GetComponentInChildren<CameraMovement>();
        cameraMove.Init();
    }

    public void ZoomCamera(float _zoomRatio)
    {
        cameraMove.ZoomCamera(_zoomRatio);
    }

    public void MoveCameraWithMouse(Vector2 _mousePos)
    {
        cameraMove.MoveCameraWithMouse(_mousePos);
    }

    public void MoveCameraWithKey(Vector2 _arrowKeyInput)
    {
        cameraMove.MoveCemeraWithKey(_arrowKeyInput);
    }

    public void MoveCameraWithObject(Vector3 _objectPos)
    {
        cameraMove.MoveCameraWithObject(_objectPos);
    }

    private CameraMovement cameraMove = null;
}
