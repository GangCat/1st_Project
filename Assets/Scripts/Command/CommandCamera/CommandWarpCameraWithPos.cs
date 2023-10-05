using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWarpCameraWithPos : Command
{
    public CommandWarpCameraWithPos(CameraMovement _cameraMove)
    {
        cameraMove = _cameraMove;
    }
    public override void Execute(params object[] _objects)
    {
        cameraMove.WarpCameraWithPos((Vector3)_objects[0]);
    }

    private CameraMovement cameraMove = null;
}
