using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandOutOneUnit : Command
{
    public CommandOutOneUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute()
    {
        selMng.OutOneUnit();
    }

    SelectableObjectManager selMng = null;
}
