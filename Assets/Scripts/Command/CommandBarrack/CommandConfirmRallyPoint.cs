using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConfirmRallyPoint : Command
{
    public CommandConfirmRallyPoint(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute()
    {
        selMng.SetRallyPoint();
    }

    private SelectableObjectManager selMng = null;
}
