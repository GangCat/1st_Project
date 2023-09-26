using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandOutAllUnit : Command
{
    public CommandOutAllUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.OutAllUnit();
    }

    SelectableObjectManager selMng = null;
}
