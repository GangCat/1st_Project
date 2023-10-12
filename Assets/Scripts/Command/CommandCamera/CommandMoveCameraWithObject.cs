using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveCameraWithObject : Command
{
    public CommandMoveCameraWithObject(CameraMovement _cameraMove)
    {
        cameraMove = _cameraMove;
    }

    public override void Execute(params object[] _objects)
    {
        cameraMove.MoveCameraWithObject(SelectableObjectManager.GetFirstSelectedObjectInList().GetPos);
    }

    private CameraMovement cameraMove = null;
}
