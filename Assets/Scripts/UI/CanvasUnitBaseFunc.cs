using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFunc
{
    public void Init()
    {
        gameObject.SetActive(false);

        btnMove.onClick.AddListener(
            ()=>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.MOVE);
            });

        btnStop.onClick.AddListener(
            () =>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.STOP);
            });

        btnHold.onClick.AddListener(
            () =>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.HOLD);
            });

        btnPatrol.onClick.AddListener(
            () =>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.PATROL);
            });

        btnAttack.onClick.AddListener(
            () =>
            {
                ArrayUnitButtonCommand.Use(EUnitButtonCommand.ATTACK);
            });

    }

    [SerializeField]
    private Button btnMove = null;
    [SerializeField]
    private Button btnStop = null;
    [SerializeField]
    private Button btnHold = null;
    [SerializeField]
    private Button btnPatrol = null;
    [SerializeField]
    private Button btnAttack = null;
}
