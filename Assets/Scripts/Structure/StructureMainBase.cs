using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMainBase : Structure
{
    public override void Init(int _structureIdx)
    {
        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());

        GetComponent<SelectableObject>().Init();
        myIdx = _structureIdx;
        upgradeLevel = 1;
    }

    public override bool StartUpgrade()
    {
        if(!isProcessingUpgrade && upgradeLevel < 3)
        {
            StartCoroutine("UpgradeCoroutine");
            return true;
        }
        return false;
    }

    protected override void UpgradeComplete()
    {
        base.UpgradeComplete();
        upgradeHpCmd.Execute(upgradeHpAmount);
        StructureManager.UpgradeLimit = upgradeLevel;

        Debug.Log("UpgradeCompleteMainBase");
    }

    [Header("-Upgrade Attribute")]
    [SerializeField]
    private float upgradeHpAmount = 0f;

    private CommandUpgradeStructureHP upgradeHpCmd = null;
}
