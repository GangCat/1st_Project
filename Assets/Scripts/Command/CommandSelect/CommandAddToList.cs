using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddToList : Command
{
    public CommandAddToList(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.AddToList((FriendlyObject)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
