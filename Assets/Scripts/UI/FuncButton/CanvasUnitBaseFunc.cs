using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUnitBaseFunc : CanvasFunc
{
    public void Init()
    {
        arrUnitFuncBtn = GetComponentsInChildren<FuncButtonBase>();
        foreach (FuncButtonBase btn in arrUnitFuncBtn)
            btn.Init();

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

    public void ChangeHotkey(EUnitFuncKey _funcHotkey, KeyCode _hotkey)
    {
        arrUnitFuncBtn[(int)_funcHotkey].SetHotkey(_hotkey);
    }

    private FuncButtonBase[] arrUnitFuncBtn = null;

}
