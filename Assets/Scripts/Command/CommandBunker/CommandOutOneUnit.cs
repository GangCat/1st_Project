using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandOutOneUnit : Command
{
    public CommandOutOneUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.OutOneUnit();
    }

    SelectableObjectManager selMng = null;
}
