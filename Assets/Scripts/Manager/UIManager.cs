using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        funcBtnMng = GetComponentInChildren<FuncButtonManager>();
        displayHUDMng = GetComponentInChildren<DisplayHUDManager>();
        funcBtnMng.Init();
        displayHUDMng.Init();
    }

    public void SpawnUnit(EUnitType _type)
    {
        displayHUDMng.SpawnUnit(_type);
    }

    public void HeroDead()
    {
        displayHUDMng.HeroDead();
    }

    public void ShowFuncButton(EObjectType _selectObjectType)
    {
        funcBtnMng.ShowFuncButton(_selectObjectType);
    }

    public void UpdateEnergy(uint _curEnergy)
    {
        displayHUDMng.UpdateEnergy(_curEnergy);
    }

    public void UpdateCore(uint _curCore)
    {
        displayHUDMng.UpdateCore(_curCore);
    }

    public void UpdateCurPopulation(uint _curPoulation)
    {
        displayHUDMng.UpdateCurPopulation(_curPoulation);
    }

    public void UpdateCurMaxPopulation(uint _curMaxPopulation)
    {
        displayHUDMng.UpdateCurMaxPopulation(_curMaxPopulation);
    }

    private FuncButtonManager funcBtnMng = null;
    private DisplayHUDManager displayHUDMng = null;
}
