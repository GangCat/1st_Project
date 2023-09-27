using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCompleteUpgradeRangedUnitDmg : Command
{
    public CommandCompleteUpgradeRangedUnitDmg(SelectableObjectManager _selMng)
    {
        selMng = _selMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.CompleteUpgradeUnitDmg();
    }

    private SelectableObjectManager selMng = null;
}
