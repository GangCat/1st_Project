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
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, EUnitType.MELEE);
            });

        btnSpawnRangedUnit.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, EUnitType.RANGED);
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
        btnRallyPoint.interactable = false;
    }

    public void SetAllButtonInteractable()
    {
        btnSpawnMeleeUnit.interactable = true;
        btnSpawnRangedUnit.interactable = true;
        btnRallyPoint.interactable = true;
    }


    [SerializeField]
    private Button btnSpawnMeleeUnit = null;
    [SerializeField]
    private Button btnSpawnRangedUnit = null;
    [SerializeField]
    private Button btnRallyPoint = null;
}