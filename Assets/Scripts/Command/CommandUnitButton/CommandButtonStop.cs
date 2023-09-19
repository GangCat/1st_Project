using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonStop : Command
{
    public CommandButtonStop(SelectableObjectManager _selectableObjMng)
    {
        selectableObjMng = _selectableObjMng;
    }

    public override void Execute(params object[] _objects)
    {
        selectableObjMng.Stop();
    }

    private SelectableObjectManager selectableObjMng = null;
}
