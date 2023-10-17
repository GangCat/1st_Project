using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmChangeBuildFuncHotkey : Command
{
    public CommandConfirmChangeBuildFuncHotkey(CanvasBuildFunc _canvasBuild)
    {
        canvasBuild = _canvasBuild;
    }

    public override void Execute(params object[] _objects)
    {
        canvasBuild.ChangeHotkey((int)_objects[0], (KeyCode)_objects[1]);
    }

    private CanvasBuildFunc canvasBuild = null;
}
