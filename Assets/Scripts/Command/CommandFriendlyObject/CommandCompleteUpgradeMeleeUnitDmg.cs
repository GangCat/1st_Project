using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCompleteUpgradeMeleeUnitDmg : Command
{
    public CommandCompleteUpgradeMeleeUnitDmg(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.CompleteUpgradeMeleeUnitDmg();
    }

    private SelectableObjectManager selMng = null;
}
