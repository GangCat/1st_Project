using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmChangeStructureFuncHotkey : Command
{
    public CommandConfirmChangeStructureFuncHotkey(CanvasStructureBaseFunc _canvasStructureFunc)
    {
        canvasStructureFunc = _canvasStructureFunc;
    }
    public override void Execute(params object[] _objects)
    {
        canvasStructureFunc.ChangeHotkey((int)_objects[0], (KeyCode)_objects[1]);
    }

    private CanvasStructureBaseFunc canvasStructureFunc = null;
}
