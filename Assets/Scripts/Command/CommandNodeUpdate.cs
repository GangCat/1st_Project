using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNodeUpdate : Command
{
    public void Execute(Vector3 _pos, int _idx)
    {
        SelectableObjectManager.UpdateNodeWalkable(_pos, _idx);
    }

    public override void Execute()
    {
    }
}
