using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandStop : Command
{
    public CommandStop(SelectableObjectManager _selectableObjMng)
    {
        selectableObjMng = _selectableObjMng;
    }

    public override void Execute()
    {
        selectableObjMng.Stop();
    }

    private SelectableObjectManager selectableObjMng = null;
}
