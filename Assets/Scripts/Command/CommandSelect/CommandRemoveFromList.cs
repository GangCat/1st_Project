using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveFromList : Command
{
    public CommandRemoveFromList(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.RemoveUnitAtList((FriendlyObject)_objects[0]);
    }
    private SelectableObjectManager selMng = null;
}
