using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        inputMng = FindAnyObjectByType<InputManager>();
        playerMng = FindAnyObjectByType<PlayerManager>();
        cameraMng = FindAnyObjectByType<CameraManager>();
    }

    private void Start()
    {
        inputMng.Init(MovePlayerByPicking, ZoomCamera, MoveCameraWithMouse);
        playerMng.Init();
        cameraMng.Init();
    }

    private void MovePlayerByPicking(Vector3 _pickPos)
    {
        playerMng.MovePlayerByPicking(_pickPos);
    }

    private void ZoomCamera(float _zoomRatio)
    {
        cameraMng.ZoomCamera(_zoomRatio);
    }

    private void MoveCameraWithMouse(Vector3 _mousePos)
    {
        cameraMng.MoveCameraWithMouse(_mousePos);
    }

    private PlayerManager playerMng = null;
    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
}
