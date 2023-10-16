using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSetListToCrowd : Command
{
    public CommandSetListToCrowd(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.SetListToCrowd((int)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
