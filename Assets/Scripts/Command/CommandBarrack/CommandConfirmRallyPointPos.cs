using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmRallyPointPos : Command
{
    public CommandConfirmRallyPointPos(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.SetRallyPoint((Vector3)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
