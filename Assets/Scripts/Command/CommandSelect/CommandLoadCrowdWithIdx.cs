using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLoadCrowdWithIdx : Command
{
    public CommandLoadCrowdWithIdx(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.LoadCrowdWithIdx((int)_objects[0]);
    }

    private SelectableObjectManager selMng = null;
}
