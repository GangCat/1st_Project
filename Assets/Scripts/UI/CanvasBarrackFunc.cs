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

        gameObject.SetActive(false);
    }

    public void SetAllButtonUninteractable()
    {
        btnSpawnMeleeUnit.interactable = false;
        btnSpawnRangedUnit.interactable = false;
        btnSpawnRocketUnit.interactable = false;
        btnRallyPoint.interactable = false;
    }

    public void SetAllButtonInteractable()
    {
        btnSpawnMeleeUnit.interactable = true;
        btnSpawnRangedUnit.interactable = true;
        btnSpawnRocketUnit.interactable = true;
        btnRallyPoint.interactable = true;
    }


    [SerializeField]
    private Button btnSpawnMeleeUnit = null;
    [SerializeField]
    private Button btnSpawnRangedUnit = null;
    [SerializeField]
    private Button btnSpawnRocketUnit = null;
    [SerializeField]
    private Button btnRallyPoint = null;
}