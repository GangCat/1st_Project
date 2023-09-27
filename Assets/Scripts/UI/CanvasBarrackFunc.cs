using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarrackFunc : CanvasFuncBase
{
    public void Init()
    {
        btnSpawnUnit1.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.MELEE);
            });

        btnSpawnUnit2.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.RANGED);
            });

        btnSpawnUnit3.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, ESpawnUnitType.ROCKET);
            });

        btnRallyPoint.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
            });

        btnUpgradeRangedUnitDmg.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_RANGED_UNIT_DMG);
            });

        btnUpgradeRangedUnitHp.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_RANGED_UNIT_HP);
            });

        btnUpgradeMeleeUnitDmg.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_MELEE_UNIT_DMG);
            });

        btnUpgradeMeleeUnitHp.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRAGE_MELEE_UNIT_HP);
            });

        gameObject.SetActive(false);
    }


    [SerializeField]
    private Button btnSpawnUnit1 = null;
    [SerializeField]
    private Button btnSpawnUnit2 = null;
    [SerializeField]
    private Button btnSpawnUnit3 = null;
    [SerializeField]
    private Button btnRallyPoint = null;
    [SerializeField]
    private Button btnUpgradeRangedUnitDmg = null;
    [SerializeField]
    private Button btnUpgradeRangedUnitHp = null;
    [SerializeField]
    private Button btnUpgradeMeleeUnitDmg = null;
    [SerializeField]
    private Button btnUpgradeMeleeUnitHp = null;
}
