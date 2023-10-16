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
        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());
        myObj = GetComponent<FriendlyObject>();
        myObj.Init();
        myStructureIdx = _structureIdx;
        upgradeLevel = 1;
        UpdateNodeWalkable(false);
    }

    public bool IsPopulationUpgrade => isPopulationUpgrade;
    public bool IsEnergySupplyUpgrade => isEnergySupplyUpgrade;

    public override bool StartUpgrade()
    {
        if(!isProcessingUpgrade && upgradeLevel < 3)
        {
            Debug.Log("structure" + upgradeLevel);
            Debug.Log("Limit" + StructureManager.UpgradeLimit);
            StartCoroutine("UpgradeCoroutine");
            return true;
        }
        Debug.Log("structure" + upgradeLevel);
        Debug.Log("Limit" + StructureManager.UpgradeLimit);
        return false;
    }

    protected override void UpgradeComplete()
    {
        base.UpgradeComplete();
        upgradeHpCmd.Execute(upgradeHpAmount);
        StructureManager.UpgradeLimit = upgradeLevel;

        //Debug.Log("UpgradeCompleteMainBase");
    }

    public override void CancleCurAction()
    {
        if (isProcessingUpgrade)
        {
            if (isPopulationUpgrade)
            {
                isProcessingUpgrade = false;
                isPopulationUpgrade = false;
                StopCoroutine("UpgradePopulationCoroutine");
                ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.UPGRADE_POPULATION);
                curUpgradeType = EUpgradeType.NONE;
            }
            else if (isEnergySupplyUpgrade)
            {
                isProcessingUpgrade = false;
                isEnergySupplyUpgrade = false;
                StopCoroutine("UpgradeEnergySupplyCoroutine");
                ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.UPGRADE_ENERGY);
                curUpgradeType = EUpgradeType.NONE;
            }
            else
            {
                StopCoroutine("UpgradeCoroutine");
                isProcessingUpgrade = false;
                ArrayRefundCurrencyCommand.Use(ERefuncCurrencyCommand.UPGRADE_STRUCTURE, myObj.GetObjectType(), upgradeLevel);
                curUpgradeType = EUpgradeType.NONE;
            }
        }
        else if (isProcessingConstruct)
        {
            StopCoroutine("BuildStructureCoroutine");
            isProcessingConstruct = false;
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH_COMPLETE, myStructureIdx);
            DestroyStructure();
        }
        else if (isProcessingDemolish)
        {
            StopCoroutine("DemolishCoroutine");
            isProcessingDemolish = false;
        }

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void UpgradeMaxPopulation()
    {
        isPopulationUpgrade = true;
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
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradePopulationDelay;
        }
        isProcessingUpgrade = false;
        isPopulationUpgrade = false; 
        curUpgradeType = EUpgradeType.NONE;
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
        isEnergySupplyUpgrade = true;
        curUpgradeType = EUpgradeType.ENERGY;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);

        float elapsedTime = 0f;
        progressPercent = elapsedTime / upgradeEnergySupplyDelay;
        while (progressPercent < 1)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
            // ui ㅠ표시
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradeEnergySupplyDelay;
        }
        isProcessingUpgrade = false;
        isEnergySupplyUpgrade = false;
        curUpgradeType = EUpgradeType.NONE;
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
    private bool isPopulationUpgrade = false;
    private bool isEnergySupplyUpgrade = false;

}
