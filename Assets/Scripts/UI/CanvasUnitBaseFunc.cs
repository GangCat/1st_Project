using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFunc
{
    public void Init()
    {
        btnMove.onClick.AddListener(
            ()=>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.MOVE);
            });

        btnStop.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.STOP);
            });

        btnHold.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.HOLD);
            });

        btnPatrol.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.PATROL);
            });

        btnAttack.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.ATTACK);
            });

        btnCancle.onClick.AddListener(
            () =>
            {
                ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.CANCLE);
            });

        gameObject.SetActive(false);
    }

    public override void SetActive(bool _isActive)
    {
        HideCancleButton();
        base.SetActive(_isActive);
    }

    public void DisplayCancleButton()
    {
        btnCancle.gameObject.SetActive(true);
    }

    public void HideCancleButton()
    {
        btnCancle.gameObject.SetActive(false);
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
    [SerializeField]
    private Button btnCancle = null;
}
