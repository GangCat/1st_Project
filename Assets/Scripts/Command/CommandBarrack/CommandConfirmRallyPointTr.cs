using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmRallyPointTr : Command
{
    public CommandConfirmRallyPointTr(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }
    public override void Execute(params object[] _objects)
    {
        selMng.SetRallyPoint((Transform)_objects[0]);
    }

    SelectableObjectManager selMng = null;
}
