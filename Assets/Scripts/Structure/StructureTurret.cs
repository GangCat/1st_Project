using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        selectObj = GetComponent<FriendlyObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
        myIdx = _structureIdx;
        upgradeDmgCmd = new CommandUpgradeAttDmg(selectObj);
        upgradeRangeCmd = new CommandUpgradeAttRange(selectObj);
    }

    public override void BuildComplete()
    {
        base.BuildComplete();
        GetComponent<FriendlyObject>().Hold();
    }

    protected override void UpgradeComplete()
    {
        upgradeDmgCmd.Execute(upgradeDmgAmount);
        upgradeRangeCmd.Execute(upgradeRangeAmount);
        Debug.Log("UpgradeCompleteTurret");
    }

    [SerializeField]
    private Transform turretHeadTr = null;
    [SerializeField]
    private float upgradeDmgAmount = 0f;
    [SerializeField]
    private float upgradeRangeAmount = 0f;

    private FriendlyObject selectObj = null;
    private CommandUpgradeAttDmg upgradeDmgCmd = null;
    private CommandUpgradeAttRange upgradeRangeCmd = null;
}
