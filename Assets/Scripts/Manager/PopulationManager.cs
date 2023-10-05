using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour, IPublisher
{
    public void Init()
    {
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, curMaxPopulation);
        RegisterBroker();
    }

    public uint CurPopulation => curPopulation;

    public bool CanSpawnUnit(EUnitType _unitType)
    {
        return curPopulation + unitPopulation[(int)_unitType] < curMaxPopulation;
    }

    private bool CanSpawnUnit()
    {
        return curPopulation < curMaxPopulation;
    }

    public bool CanUpgradePopulation()
    {
        return curMaxPopulation < maxPopulation;
    }

    public void SpawnUnit(EUnitType _unitType)
    {
        IncreaseCurPopulation(unitPopulation[(int)_unitType]);
    }

    public void UnitDead(EUnitType _unitType)
    {
        DecreasePopulation(unitPopulation[(int)_unitType]);
    }

    private void IncreaseCurPopulation(uint _increaseAmount)
    {
        curPopulation += _increaseAmount;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
        if (!CanSpawnUnit())
            PushMessageToBroker(EMessageType.STOP_SPAWN);
    }

    public void DecreasePopulation(uint _decreaseAmount)
    {
        curPopulation -= _decreaseAmount;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, curPopulation);
        if (CanSpawnUnit())
            PushMessageToBroker(EMessageType.START_SPAWN);
    }
    
    public void UpgradeMaxPopulation()
    {
        curMaxPopulation += 20;
        ArrayPopulationCommand.Use(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, curMaxPopulation);
        if (CanSpawnUnit())
            PushMessageToBroker(EMessageType.START_SPAWN);
    }

    public void RegisterBroker()
    {
        Broker.Regist(this, EPublisherType.POPULATION_MANAGER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.POPULATION_MANAGER);
    }

    [SerializeField]
    private uint maxPopulation = 0;
    [SerializeField]
    private uint curMaxPopulation = 0;
    [SerializeField]
    private uint curPopulation = 0;
    [SerializeField]
    private uint[] unitPopulation = null;

    private bool canSpawnUnit = true;
}
