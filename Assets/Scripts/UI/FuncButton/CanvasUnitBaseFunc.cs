using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFunc
{
    public void Init()
    {
        arrUnitFuncBtn = new FuncButtonBase[(int)EUnitFuncKey.LENGTH];

        arrUnitFuncBtn[0] = buttonMoveUnit;
        arrUnitFuncBtn[1] = buttonStopUnit;
        arrUnitFuncBtn[2] = buttonHoldUnit;
        arrUnitFuncBtn[3] = buttonPatrolUnit;
        arrUnitFuncBtn[4] = buttonAttackUnit;
        arrUnitFuncBtn[5] = buttonLaunchNuclear;
        arrUnitFuncBtn[6] = buttonCancleUnit;

        for(int i = 0; i < arrUnitFuncBtn.Length; ++i)
            arrUnitFuncBtn[i].Init();

        gameObject.SetActive(false);
    }

    protected override void SetActive(bool _isActive)
    {
        HideCancleButton();
        base.SetActive(_isActive);
    }

    public void DisplayCancleButton()
    {
        arrUnitFuncBtn[(int)EUnitFuncKey.CANCLE].SetActive(true);
    }

    public void HideCancleButton()
    {
        arrUnitFuncBtn[(int)EUnitFuncKey.CANCLE].SetActive(false);
    }

    public void DisplayLaunchNuclearButton()
    {
        arrUnitFuncBtn[(int)EUnitFuncKey.LAUNCH_NUCLEAR].SetActive(true);
    }

    public void HideLaunchNuclearButton()
    {
        arrUnitFuncBtn[(int)EUnitFuncKey.LAUNCH_NUCLEAR].SetActive(false);
    }

    public void ChangeHotkey(int _funcKeyIdx, KeyCode _hotkey)
    {
        arrUnitFuncBtn[_funcKeyIdx].SetHotkey(_hotkey);
    }

    [SerializeField]
    private ButtonMoveUnit buttonMoveUnit = null;
    [SerializeField]
    private ButtonStopUnit buttonStopUnit = null;
    [SerializeField]
    private ButtonHoldUnit buttonHoldUnit = null;
    [SerializeField]
    private ButtonPatrolUnit buttonPatrolUnit = null;
    [SerializeField]
    private ButtonAttackUnit buttonAttackUnit = null;
    [SerializeField]
    private ButtonLaunchNuclear buttonLaunchNuclear = null;
    [SerializeField]
    private ButtonCancleUnit buttonCancleUnit = null;


    private FuncButtonBase[] arrUnitFuncBtn = null;

}
