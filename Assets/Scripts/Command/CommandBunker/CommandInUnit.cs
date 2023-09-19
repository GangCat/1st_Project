using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInUnit : Command
{
    public CommandInUnit(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _value)
    {
        FriendlyObject tempObj = (FriendlyObject)_value[0];
        selMng.InUnit(tempObj);
        selMng.RemoveUnitAtList(tempObj);
        selMng.ResetTargetBunker();
    }

    SelectableObjectManager selMng = null;
}
