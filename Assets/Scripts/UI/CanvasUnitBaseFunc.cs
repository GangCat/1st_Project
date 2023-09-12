using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFuncBase
{
    public void Init()
    {
        gameObject.SetActive(false);

        btnMove.onClick.AddListener(
            ()=>
            {
                ListUnitButtonCommand.Use((int)EUnitButtonCommand.MOVE);
            });

        btnStop.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use((int)EUnitButtonCommand.STOP);
            });

        btnHold.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use((int)EUnitButtonCommand.HOLD);
            });

        btnPatrol.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use((int)EUnitButtonCommand.PATROL);
            });

        btnAttack.onClick.AddListener(
            () =>
            {
                ListUnitButtonCommand.Use((int)EUnitButtonCommand.ATTACK);
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
