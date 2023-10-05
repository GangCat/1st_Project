using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMainBase : Structure
{
    public override void Init(PF_Grid _grid)
    {
        grid = _grid;
    }

    public override void Init(int _structureIdx)
    {
        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());

        GetComponent<FriendlyObject>().Init();
        myIdx = _structureIdx;
        upgradeLevel = 1;
        UpdateNodeWalkable(false);
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
        StartCoroutine("UpgradePopulationCoroutine");
    }

    private IEnumerator UpgradePopulationCoroutine()
    {
        isProcessingUpgrade = true;
        curUpgradeType = EUpgradeType.POPULATION;
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.DISPLAY_UPGRADE_INFO, curUpgradeType);

        float upgradeFinishTime = Time.time + upgradePopulationDelay;
        while(upgradeFinishTime > Time.time)
        {
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
        }
        isProcessingUpgrade = false;
        ArrayPopulationCommand.Use(EPopulationCommand.UPGRADE_POPULATION_COMPLETE);
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.FINISH);
        ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void UpgradeEnergySupply()
    {
        StartCoroutine("UpgradeEnergySupplyCoroutine");
    }

    private IEnumerator UpgradeEnergySupplyCoroutine()
    {
        isProcessingUpgrade = true;
        curUpgradeType = EUpgradeType.ENERGY;
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.DISPLAY_UPGRADE_INFO, curUpgradeType);

        float upgradeFinishTime = Time.time + upgradeEnergySupplyDelay;
        while (upgradeFinishTime > Time.time)
        {
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
        }
        isProcessingUpgrade = false;
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY_COMPLETE);
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.FINISH);
        ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
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
