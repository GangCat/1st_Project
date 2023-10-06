using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveCameraWithMousePos : Command
{
    public CommandMoveCameraWithMousePos(CameraMovement _cameraMove)
    {
        cameraMove = _cameraMove;
    }
    public override void Execute(params object[] _objects)
    {
        cameraMove.MoveCameraWithMouse((Vector3)_objects[0]);
    }

    private CameraMovement cameraMove = null;
}
