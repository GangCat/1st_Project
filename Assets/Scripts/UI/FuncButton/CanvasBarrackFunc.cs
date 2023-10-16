using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBarrackFunc : CanvasFunc
{
    public void Init()
    {
        arrBarrackFuncBtn = new FuncButtonBase[(int)EBarrackFuncKey.LENGTH];

        arrBarrackFuncBtn[0] = buttonSpawnMeleeUnit;
        arrBarrackFuncBtn[1] = buttonSpawnRangedUnit;
        arrBarrackFuncBtn[2] = buttonSetRallyPoint;
        arrBarrackFuncBtn[3] = buttonUpgradeRangedUnitDmg;
        arrBarrackFuncBtn[4] = buttonUpgradeRangedUnitHp;
        arrBarrackFuncBtn[5] = buttonUpgradeMeleeUnitDmg;
        arrBarrackFuncBtn[6] = buttonUpgradeMeleeUnitHp;

        for(int i = 0; i < arrBarrackFuncBtn.Length; ++i)
            arrBarrackFuncBtn[i].Init();

        gameObject.SetActive(false);
    }

    public void ChangeHotkey(int _funcKeyIdx, KeyCode _hotkey)
    {
        arrBarrackFuncBtn[_funcKeyIdx].SetHotkey(_hotkey);
    }

    [SerializeField]
    private ButtonSpawnMeleeUnit buttonSpawnMeleeUnit = null;
    [SerializeField]
    private ButtonSpawnRangedUnit buttonSpawnRangedUnit = null;
    [SerializeField]
    private ButtonSetRallyPoint buttonSetRallyPoint = null;
    [SerializeField]
    private ButtonUpgradeMeleeUnitDmg buttonUpgradeMeleeUnitDmg = null;
    [SerializeField]
    private ButtonUpgradeMeleeUnitHp buttonUpgradeMeleeUnitHp = null;
    [SerializeField]
    private ButtonUpgradeRangedUnitDmg buttonUpgradeRangedUnitDmg = null;
    [SerializeField]
    private ButtonUpgradeRangedUnitHp buttonUpgradeRangedUnitHp = null;

    private FuncButtonBase[] arrBarrackFuncBtn = null;
}