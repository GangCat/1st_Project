using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveCameraWithKey : Command
{
    public CommandMoveCameraWithKey(CameraMovement _camMove)
    {
        camMove = _camMove;
    }

    public override void Execute(params object[] _objects)
    {
        camMove.MoveCameraWithKey((Vector2)_objects[0]);
    }

    private CameraMovement camMove = null;
}
