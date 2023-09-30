using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarrackFunc : CanvasFunc
{
    public void Init()
    {
        btnSpawnMeleeUnit.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.MELEE);
            });

        btnSpawnRangedUnit.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.RANGED);
            });

        btnSpawnRocketUnit.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.ROCKET);
            });

        btnRallyPoint.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
            });

        //btnUpgradeRangedUnitDmg.onClick.AddListener(
        //    () =>
        //    {
        //        ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_RANGED_UNIT_DMG);
        //    });

        //btnUpgradeRangedUnitHp.onClick.AddListener(
        //    () =>
        //    {
        //        ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_RANGED_UNIT_HP);
        //    });

        //btnUpgradeMeleeUnitDmg.onClick.AddListener(
        //    () =>
        //    {
        //        ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_MELEE_UNIT_DMG);
        //    });

        //btnUpgradeMeleeUnitHp.onClick.AddListener(
        //    () =>
        //    {
        //        ArrayBarrackCommand.Use(EBarrackCommand.UPGRAGE_MELEE_UNIT_HP);
        //    });

        gameObject.SetActive(false);
    }

    public void SetAllButtonUninteractable()
    {
        btnSpawnMeleeUnit.interactable = false;
        btnSpawnRangedUnit.interactable = false;
        btnSpawnRocketUnit.interactable = false;
        btnRallyPoint.interactable = false;
        //btnUpgradeRangedUnitDmg.interactable = false;
        //btnUpgradeRangedUnitHp.interactable = false;
        //btnUpgradeMeleeUnitDmg.interactable = false;
        //btnUpgradeMeleeUnitHp.interactable = false;
    }

    public void SetAllButtonInteractable()
    {
        btnSpawnMeleeUnit.interactable = true;
        btnSpawnRangedUnit.interactable = true;
        btnSpawnRocketUnit.interactable = true;
        btnRallyPoint.interactable = true;
        //btnUpgradeRangedUnitDmg.interactable = true;
        //btnUpgradeRangedUnitHp.interactable = true;
        //btnUpgradeMeleeUnitDmg.interactable = true;
        //btnUpgradeMeleeUnitHp.interactable = true;
    }

    //public void SetUpgradeButtonUninteractable()
    //{
    //    btnUpgradeRangedUnitDmg.interactable = false;
    //    btnUpgradeRangedUnitHp.interactable = false;
    //    btnUpgradeMeleeUnitDmg.interactable = false;
    //    btnUpgradeMeleeUnitHp.interactable = false;
    //}

    //public void SetUpgradeButtonInteractable()
    //{
    //    btnUpgradeRangedUnitDmg.interactable = true;
    //    btnUpgradeRangedUnitHp.interactable = true;
    //    btnUpgradeMeleeUnitDmg.interactable = true;
    //    btnUpgradeMeleeUnitHp.interactable = true;
    //}


    [SerializeField]
    private Button btnSpawnMeleeUnit = null;
    [SerializeField]
    private Button btnSpawnRangedUnit = null;
    [SerializeField]
    private Button btnSpawnRocketUnit = null;
    [SerializeField]
    private Button btnRallyPoint = null;
    //[SerializeField]
    //private Button btnUpgradeRangedUnitDmg = null;
    //[SerializeField]
    //private Button btnUpgradeRangedUnitHp = null;
    //[SerializeField]
    //private Button btnUpgradeMeleeUnitDmg = null;
    //[SerializeField]
    //private Button btnUpgradeMeleeUnitHp = null;
}