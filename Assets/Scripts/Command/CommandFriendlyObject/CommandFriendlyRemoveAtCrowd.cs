using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFriendlyRemoveAtCrowd : Command
{
    public CommandFriendlyRemoveAtCrowd(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.RemoveAtCrowd((int)_objects[0], (FriendlyObject)_objects[1]);
    }

    private SelectableObjectManager selMng = null;
}
