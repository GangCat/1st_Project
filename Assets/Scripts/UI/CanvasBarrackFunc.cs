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

        btnUpgradeUnitDmg.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT_DMG);
            });

        btnUpgradeUnitHp.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT_HP);
            });

        btnRallyPoint.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
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
    private Button btnUpgradeUnitDmg = null;
    [SerializeField]
    private Button btnUpgradeUnitHp = null;
    [SerializeField]
    private Button btnRallyPoint = null;
}
