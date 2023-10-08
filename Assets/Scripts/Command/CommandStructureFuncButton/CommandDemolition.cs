using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDemolition : Command
{
    public override void Execute(params object[] _objects)
    {
        SelectableObjectManager.GetFirstSelectedObjectInList.GetComponent<Structure>().Demolish();
    }
}
