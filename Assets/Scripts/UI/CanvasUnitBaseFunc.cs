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
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.MOVE);
            });

        btnStop.onClick.AddListener(
            () =>
            {
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.STOP);
            });

        btnHold.onClick.AddListener(
            () =>
            {
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.HOLD);
            });

        btnPatrol.onClick.AddListener(
            () =>
            {
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.PATROL);
            });

        btnAttack.onClick.AddListener(
            () =>
            {
                ArrayFuncButtonCommand.Use(EFuncButtonCommand.ATTACK);
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
