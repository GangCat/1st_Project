using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasStructureBaseFunc : CanvasFunc
{
    public void Init()
    {
        arrStructureFuncBtn = new FuncButtonBase[(int)EStructureFuncKey.LENGTH];
        arrStructureFuncBtn[0] = buttonUpgradeStructure;
        arrStructureFuncBtn[1] = buttonDemolishStructure;
        arrStructureFuncBtn[2] = buttonSpawnNuclear;
        arrStructureFuncBtn[3] = buttonOutOneUnit;
        arrStructureFuncBtn[4] = buttonOutAllUnit;
        arrStructureFuncBtn[5] = buttonUpgradeEnergySupply;
        arrStructureFuncBtn[6] = buttonUpgradePopulation;
        arrStructureFuncBtn[7] = buttonStructureCancle;

        for (int i = 0; i < arrStructureFuncBtn.Length; ++i)
        {
            arrStructureFuncBtn[i].Init();
            arrStructureFuncBtn[i].SetActive(false);
        }

        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE].SetActive(true);
        arrStructureFuncBtn[(int)EStructureFuncKey.DEMOLISH].SetActive(true);

        gameObject.SetActive(false);
    }

    public void ChangeHotkey(int _funcKeyIdx, KeyCode _hotkey)
    {
        arrStructureFuncBtn[_funcKeyIdx].SetHotkey(_hotkey);
    }

    public void ChangeUpgradeStructureCost(int _cost)
    {
        buttonUpgradeStructure.SetCost(_cost);
    }

    public override void DisplayCanvas()
    {
        SetActive(true);
        buttonUpgradeStructure.SetCost(
            (int)CurrencyManager.UpgradeCost(SelectableObjectManager.GetFirstSelectedObjectInList().GetObjectType()) * 
            SelectableObjectManager.GetFirstSelectedObjectInList().GetComponent<Structure>().UpgradeLevel
        );
        HideCancleButton();
    }

    public void DisplayBunkerFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.OUT_ONE_UNIT].SetActive(true);
        arrStructureFuncBtn[(int)EStructureFuncKey.OUT_ALL_UNIT].SetActive(true);
    }

    public void HideBunkerFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.OUT_ONE_UNIT].SetActive(false);
        arrStructureFuncBtn[(int)EStructureFuncKey.OUT_ALL_UNIT].SetActive(false);
    }

    public void DisplayMainbaseFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_ENERGY_SUPPLY].SetCost((int)CurrencyManager.UpgradeETCCost(EUpgradeETCType.ENERGY_SUPPLY));
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_POPULATION_MAX].SetCost((int)CurrencyManager.UpgradeETCCost(EUpgradeETCType.CURRENT_MAX_POPULATION));

        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_ENERGY_SUPPLY].SetActive(true);
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_POPULATION_MAX].SetActive(true);
    }

    public void HideMainbaseFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_ENERGY_SUPPLY].SetActive(false);
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE_POPULATION_MAX].SetActive(false);
    }

    public void DisplayNuclearStructureFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.SPAWN_NUCLEAR].SetActive(true);
    }

    public void HideNuclearStructureFunc()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.SPAWN_NUCLEAR].SetActive(false);
    }

    public void DisplayCancleButton()
    {
        SetActive(true);
        for (int i = 0; i < arrStructureFuncBtn.Length; ++i)
            arrStructureFuncBtn[i].SetActive(false);

        arrStructureFuncBtn[(int)EStructureFuncKey.CANCLE].SetActive(true);
    }

    public void HideCancleButton()
    {
        arrStructureFuncBtn[(int)EStructureFuncKey.UPGRADE].SetActive(true);    
        arrStructureFuncBtn[(int)EStructureFuncKey.DEMOLISH].SetActive(true);
        arrStructureFuncBtn[(int)EStructureFuncKey.CANCLE].SetActive(false);
    }

    [SerializeField]
    private ButtonUpgradeStructure buttonUpgradeStructure = null;
    [SerializeField]
    private ButtonDemolishStructure buttonDemolishStructure = null;
    [SerializeField]
    private ButtonSpawnNuclear buttonSpawnNuclear = null;
    [SerializeField]
    private ButtonOutOneUnit buttonOutOneUnit = null;
    [SerializeField]
    private ButtonOutAllUnit buttonOutAllUnit = null;
    [SerializeField]
    private ButtonUpgradeEnergySupply buttonUpgradeEnergySupply = null;
    [SerializeField]
    private ButtonUpgradePopulation buttonUpgradePopulation= null;
    [SerializeField]
    private ButtonStructureCancle buttonStructureCancle = null;

    private FuncButtonBase[] arrStructureFuncBtn = null;
}
