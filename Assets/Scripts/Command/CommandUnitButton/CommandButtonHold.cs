using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonHold : Command
{
    public CommandButtonHold(SelectableObjectManager _selectMng)
    {
        selectMng = _selectMng;
    }

    public override void Execute()
    {
        selectMng.Hold();
    }

    private SelectableObjectManager selectMng = null;
}
