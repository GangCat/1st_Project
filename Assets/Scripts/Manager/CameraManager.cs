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

    private CameraMovement cameraMove = null;
}
