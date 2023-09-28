using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public void Init()
    {
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_ENERGY_HUD, curEnergy);
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_CORE_HUD, curCore);
        StartCoroutine("SupplyEnergyCoroutine");
    }

    private IEnumerator SupplyEnergyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(energySupplyRate);
            curEnergy = Functions.ClampMaxWithUInt(curEnergy + energyIncreaseAmount, maxEnergy);
            UpdateEnergy();
        }
    }


    public void IncreaseCore(uint _increaseCore)
    {
        curCore = Functions.ClampMaxWithUInt(curCore + _increaseCore, maxCore);
        UpdateCore();
    }

    public bool DecreaseEnergy(uint _decreaseEnergy)
    {
        if (curEnergy < _decreaseEnergy) return false;

        curEnergy -= _decreaseEnergy;
        UpdateEnergy();
        return true;
    }

    public bool DecreaseCore(uint _decreaseCore)
    {
        if (curCore < _decreaseCore) return false;

        curCore -= _decreaseCore;
        UpdateCore();
        return true;
    }

    private void UpdateEnergy()
    {
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_ENERGY_HUD, curEnergy);
    }

    private void UpdateCore()
    {
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_CORE_HUD, curCore);
    }


    [SerializeField]
    private uint energyIncreaseAmount = 0;
    [SerializeField]
    private uint curEnergy = 300;
    [SerializeField]
    private uint maxEnergy = 100000;
    [SerializeField, Range(1f, 30f)]
    private float energySupplyRate = 1f;
    [SerializeField]
    private uint curCore = 0;
    [SerializeField]
    private uint maxCore = 100000;
}
