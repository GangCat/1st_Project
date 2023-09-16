using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarrackFunc : CanvasFuncBase
{
    public void Init()
    {
        buttonSpawnUnit1.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_MELEE);
            });

        buttonSpawnUnit2.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_RANGE);
            });

        buttonSpawnUnit3.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_ROCKET);
            });

        buttonRallyPoint.onClick.AddListener(
            () =>
            {
                ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
            });

        gameObject.SetActive(false);
    }


    [SerializeField]
    private Button buttonSpawnUnit1 = null;
    [SerializeField]
    private Button buttonSpawnUnit2 = null;
    [SerializeField]
    private Button buttonSpawnUnit3 = null;
    [SerializeField]
    private Button buttonRallyPoint = null;
}
