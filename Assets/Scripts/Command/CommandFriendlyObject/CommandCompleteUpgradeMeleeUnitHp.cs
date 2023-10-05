using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCompleteUpgradeMeleeUnitHp : Command
{
    public CommandCompleteUpgradeMeleeUnitHp(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.CompleteUpgradeMeleeUnitHp();
        selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
}
