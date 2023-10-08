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
        myObj = GetComponent<FriendlyObject>();
        myObj.Init();
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
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);

        float elapsedTime = 0f;
        progressPercent = elapsedTime / upgradePopulationDelay;
        while(progressPercent < 1)
        {
            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradePopulationDelay;
        }
        isProcessingUpgrade = false;
        ArrayPopulationCommand.Use(EPopulationCommand.UPGRADE_POPULATION_COMPLETE);
        if(myObj.IsSelect)
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
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);

        float elapsedTime = 0f;
        progressPercent = elapsedTime / upgradeEnergySupplyDelay;
        while (progressPercent < 1)
        {
            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradeEnergySupplyDelay;
        }
        isProcessingUpgrade = false;
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY_COMPLETE);
        if (myObj.IsSelect)
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
