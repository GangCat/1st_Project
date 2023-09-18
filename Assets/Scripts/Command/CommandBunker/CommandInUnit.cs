using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInUnit : Command
{
    public CommandInUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute()
    {
        SelectableObject tempObj = selMng.InUnit();
        selMng.RemoveUnitAtList(tempObj);
    }

    SelectableObjectManager selMng = null;
}
