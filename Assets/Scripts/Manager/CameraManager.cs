using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void Init()
    {
        cameraMove = GetComponentInChildren<CameraMovement>();
        cameraMove.Init();

        ArrayCameraMoveCommand.Add(ECameraCommand.WARP_WITH_POS, new CommandWarpCameraWithPos(cameraMove));
        ArrayCameraMoveCommand.Add(ECameraCommand.MOVE_WITH_MOUSE, new CommandMoveCameraWithMousePos(cameraMove));
        ArrayCameraMoveCommand.Add(ECameraCommand.MOVE_WITH_KEY, new CommandMoveCameraWithKey(cameraMove));
        ArrayCameraMoveCommand.Add(ECameraCommand.ZOOM, new CommandZoomCamera(cameraMove));
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
        cameraMove.MoveCameraWithKey(_arrowKeyInput);
    }

    public void MoveCameraWithObject(Vector3 _objectPos)
    {
        cameraMove.MoveCameraWithObject(_objectPos);
    }

    private CameraMovement cameraMove = null;
}
