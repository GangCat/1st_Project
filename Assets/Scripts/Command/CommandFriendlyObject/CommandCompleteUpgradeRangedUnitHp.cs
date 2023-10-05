using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCompleteUpgradeRangedUnitHp : Command
{
    public CommandCompleteUpgradeRangedUnitHp(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.CompleteUpgradeRangedUnitHp();
        selMng.UpdateFuncButton();
    }

    private SelectableObjectManager selMng = null;
}
