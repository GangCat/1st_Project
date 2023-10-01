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

    public void UpgradeMaxPopulation()
    {
        if(!isProcessingUpgrade)
            StartCoroutine("UpgradePopulationCoroutine");
    }

    private IEnumerator UpgradePopulationCoroutine()
    {
        isProcessingUpgrade = true;
        float upgradeFinishTime = Time.time + upgradePopulationDelay;
        while(upgradeFinishTime > Time.time)
        {
            // ui ��ǥ��
            yield return new WaitForSeconds(0.5f);
        }
        isProcessingUpgrade = false;
        ArrayPopulationCommand.Use(EPopulationCommand.UPGRADE_POPULATION_COMPLETE);
    }

    [Header("-Upgrade Attribute")]
    [SerializeField]
    private float upgradeHpAmount = 0f;
    [SerializeField]
    private float upgradePopulationDelay = 10f;
    [SerializeField]
    private float upgradeEnergySupplyDelay = 10f;

    private CommandUpgradeStructureHP upgradeHpCmd = null;
}
