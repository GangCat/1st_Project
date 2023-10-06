using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandZoomCamera : Command
{
    public CommandZoomCamera(CameraMovement _cameraMove)
    {
        cameraMove = _cameraMove;
    }

    public override void Execute(params object[] _objects)
    {
        cameraMove.ZoomCamera((float)_objects[0]);
    }

    private CameraMovement cameraMove = null;
}
