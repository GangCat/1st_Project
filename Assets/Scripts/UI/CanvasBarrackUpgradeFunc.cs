using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarrackUpgradeFunc : CanvasFunc
{
    public void Init()
    {
        btnUpgradeRangedUnitDmg.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.RANGED_UNIT_DMG);
            });

        btnUpgradeRangedUnitHp.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.RANGED_UNIT_HP);
            });

        btnUpgradeMeleeUnitDmg.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.MELEE_UNIT_DMG);
            });

        btnUpgradeMeleeUnitHp.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.MELEE_UNIT_HP);
            });
    }

    public void SetAllButtonUninteractable()
    {
        btnUpgradeRangedUnitDmg.interactable = false;
        btnUpgradeRangedUnitHp.interactable = false;
        btnUpgradeMeleeUnitDmg.interactable = false;
        btnUpgradeMeleeUnitHp.interactable = false;
    }

    public void SetAllButtonInteractable()
    {
        btnUpgradeRangedUnitDmg.interactable = true;
        btnUpgradeRangedUnitHp.interactable = true;
        btnUpgradeMeleeUnitDmg.interactable = true;
        btnUpgradeMeleeUnitHp.interactable = true;
    }


    [SerializeField]
    private Button btnUpgradeRangedUnitDmg = null;
    [SerializeField]
    private Button btnUpgradeRangedUnitHp = null;
    [SerializeField]
    private Button btnUpgradeMeleeUnitDmg = null;
    [SerializeField]
    private Button btnUpgradeMeleeUnitHp = null;
}
