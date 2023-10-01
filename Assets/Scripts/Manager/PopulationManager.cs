using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public void Init()
    {
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, curMaxPopulation);
    }

    public uint CurPopulation => curPopulation;

    public bool CanSpawnUnit(ESpawnUnitType _unitType)
    {
        return curPopulation + unitPopulation[(int)_unitType] < curMaxPopulation;
    }

    public bool CanUpgradePopulation()
    {
        return curMaxPopulation < maxPopulation;
    }

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        IncreaseCurPopulation(unitPopulation[(int)_unitType]);
    }

    public void UnitDead(ESpawnUnitType _unitType)
    {
        DecreasePopulation(unitPopulation[(int)_unitType]);
    }

    private void IncreaseCurPopulation(uint _increaseAmount)
    {
        curPopulation += _increaseAmount;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
    }

    public void DecreasePopulation(uint _decreaseAmount)
    {
        curPopulation -= _decreaseAmount;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
    }
    
    public void UpgradeMaxPopulation()
    {
        curMaxPopulation += 20;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, curMaxPopulation);
    }




    [SerializeField]
    private uint maxPopulation = 0;
    [SerializeField]
    private uint curMaxPopulation = 0;
    [SerializeField]
    private uint curPopulation = 0;
    [SerializeField]
    private uint[] unitPopulation = null;
}
