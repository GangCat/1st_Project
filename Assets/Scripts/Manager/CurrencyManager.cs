using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IPublisher
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

    public void RegisterBroker()
    {
        Broker.Regist(this, EPublisherType.ENERGY_UPDATE);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.ENERGY_UPDATE);
    }

    public bool BuildStructure(EObjectType _objType)
    {
        switch (_objType)
        {
            case EObjectType.TURRET:
                return DecreaseEnergy(buildTurret);
            case EObjectType.BUNKER:
                return DecreaseEnergy(buildBunker);
            case EObjectType.WALL:
                return DecreaseEnergy(buildWall);
            case EObjectType.BARRACK:
                return DecreaseEnergy(buildBarrack);
            case EObjectType.NUCLEAR:
                return DecreaseEnergy(buildNuclear);
            default:
                return false;
        }
    }

    public bool UpgradeStructure(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                return DecreaseCore(upgradeMainBase * (uint)_level);
            case EObjectType.TURRET:
                return DecreaseCore(upgradeTurret * (uint)_level);
            case EObjectType.BUNKER:
                return DecreaseCore(upgradeBunker * (uint)_level);
            case EObjectType.WALL:
                return DecreaseCore(upgradeWall * (uint)_level);
            case EObjectType.BARRACK:
                return DecreaseCore(upgradeBarrack * (uint)_level);
            default:
                return false;
        }
    }

    public bool UpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                return DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
            case EUnitUpgradeType.RANGED_UNIT_HP:
                return DecreaseCore(upgradeUnitHp * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                return DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_HP:
                return DecreaseCore(upgradeUnitHp * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
            default:
                return false;
        }
    }

    public bool SpawnUnit(ESpawnUnitType _unitType)
    {
        switch (_unitType)
        {
            case ESpawnUnitType.MELEE:
                return DecreaseEnergy(spawnMeleeUnit);
            case ESpawnUnitType.RANGED:
                return DecreaseEnergy(spawnRangedUnit);
            case ESpawnUnitType.ROCKET:
                return false;
            default:
                return false;
        }
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

    [Header("-Energy")]
    [Header("-Build Structure Cost")]
    [SerializeField]
    private uint buildBarrack = 150;
    [SerializeField]
    private uint buildBunker = 100;
    [SerializeField]
    private uint buildWall = 50;
    [SerializeField]
    private uint buildTurret = 150;
    [SerializeField]
    private uint buildNuclear = 50;

    [Header("-Spawn Unit Cost")]
    [SerializeField]
    private uint spawnMeleeUnit = 70;
    [SerializeField]
    private uint spawnRangedUnit = 50;
    [SerializeField]
    private uint spawnRocketUnit = 0;

    [Header("-Core")]
    [Header("-Upgrade Unit Cost")]
    [SerializeField]
    private uint upgradeUnitHp = 50;
    [SerializeField]
    private uint upgradeUnitDmg = 50;

    [Header("-UpgradeStructureCose")]
    [SerializeField]
    private uint upgradeMainBase = 200;
    [SerializeField]
    private uint upgradeBarrack = 100;
    [SerializeField]
    private uint upgradeBunker = 70;
    [SerializeField]
    private uint upgradeWall = 30;
    [SerializeField]
    private uint upgradeTurret = 100;

}