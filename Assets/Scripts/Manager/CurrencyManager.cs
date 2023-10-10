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
            curEnergy = Functions.ClampMaxWithUInt(curEnergy + energySupplyAmount, maxEnergy);
            UpdateEnergy();
        }
    }

    public void IncreaseCore(uint _increaseCore)
    {
        curCore = Functions.ClampMaxWithUInt(curCore + _increaseCore, maxCore);
        UpdateCore();
    }

    private void DecreaseEnergy(uint _decreaseEnergy)
    {
        curEnergy -= _decreaseEnergy;
        UpdateEnergy();
    }

    private void DecreaseCore(uint _decreaseCore)
    {
        curCore -= _decreaseCore;
        UpdateCore();
    }

    private bool IsCoreEnough(uint _decreaseCore)
    {
        if (curCore < _decreaseCore) return false;
        return true;
    }

    private bool IsEnergyEnough(uint _decreaseEnergy)
    {
        if (curEnergy < _decreaseEnergy) return false;
        return true;
    }

    public void UpgradeEnergySupply()
    {
        energySupplyAmount += 10;
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

    public bool CanBuildStructure(EObjectType _objType)
    {
        switch (_objType)
        {
            case EObjectType.TURRET:
                return IsEnergyEnough(buildTurret);
            case EObjectType.BUNKER:
                return IsEnergyEnough(buildBunker);
            case EObjectType.WALL:
                return IsEnergyEnough(buildWall);
            case EObjectType.BARRACK:
                return IsEnergyEnough(buildBarrack);
            case EObjectType.NUCLEAR:
                return IsEnergyEnough(buildNuclear);
            default:
                return false;
        }
    }

    public void BuildStructure(EObjectType _objType)
    {
        switch (_objType)
        {
            case EObjectType.TURRET:
                DecreaseEnergy(buildTurret);
                break;
            case EObjectType.BUNKER:
                DecreaseEnergy(buildBunker);
                break;
            case EObjectType.WALL:
                DecreaseEnergy(buildWall);
                break;
            case EObjectType.BARRACK:
                DecreaseEnergy(buildBarrack);
                break;
            case EObjectType.NUCLEAR:
                DecreaseEnergy(buildNuclear);
                break;
            default:
                break;
        }
    }

    public bool CanUpgradeSturcture(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                return IsCoreEnough(upgradeMainBase * (uint)_level);
            case EObjectType.TURRET:
                return IsCoreEnough(upgradeTurret * (uint)_level);
            case EObjectType.BUNKER:
                return IsCoreEnough(upgradeBunker * (uint)_level);
            case EObjectType.WALL:
                return IsCoreEnough(upgradeWall * (uint)_level);
            case EObjectType.BARRACK:
                return IsCoreEnough(upgradeBarrack * (uint)_level);
            default:
                return false;
        }
    }

    public void UpgradeStructure(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                DecreaseCore(upgradeMainBase * (uint)_level);
                break;
            case EObjectType.TURRET:
                DecreaseCore(upgradeTurret * (uint)_level);
                break;
            case EObjectType.BUNKER:
                DecreaseCore(upgradeBunker * (uint)_level);
                break;
            case EObjectType.WALL:
                DecreaseCore(upgradeWall * (uint)_level);
                break;
            case EObjectType.BARRACK:
                DecreaseCore(upgradeBarrack * (uint)_level);
                break;
            default:
                break;
        }
    }

    public bool CanUpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                return IsCoreEnough(upgradeUnitDmg * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
            case EUnitUpgradeType.RANGED_UNIT_HP:
                return IsCoreEnough(upgradeUnitHp * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                return IsCoreEnough(upgradeUnitDmg * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_HP:
                return IsCoreEnough(upgradeUnitHp * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
            default:
                return false;
        }
    }

    public void UpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                DecreaseCore(upgradeUnitDmg * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
                break;
            default:
                break;
        }
    }

    public bool CanSpawnUnit(EUnitType _unitType)
    {
        switch (_unitType)
        {
            case EUnitType.MELEE:
                return IsEnergyEnough(spawnMeleeUnit);
            case EUnitType.RANGED:
                return IsEnergyEnough(spawnRangedUnit);
            default:
                return false;
        }
    }

    public void SpawnUnit(EUnitType _unitType)
    {
        switch (_unitType)
        {
            case EUnitType.MELEE:
                DecreaseEnergy(spawnMeleeUnit);
                break;
            case EUnitType.RANGED:
                DecreaseEnergy(spawnRangedUnit);
                break;
            default:
                break;
        }
    }

    public bool CanUpgradeETC(EUpgradeETCType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUpgradeETCType.CURRENT_MAX_POPULATION:
                return IsCoreEnough(upgradeMaxPopulation);
            case EUpgradeETCType.ENERGY_SUPPLY:
                return IsCoreEnough(upgradeEnergySupply);
            default:
                return false;
        }
    }

    public void UpgradeETC(EUpgradeETCType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUpgradeETCType.CURRENT_MAX_POPULATION:
                DecreaseCore(upgradeMaxPopulation);
                break;
            case EUpgradeETCType.ENERGY_SUPPLY:
                DecreaseCore(upgradeEnergySupply);
                break;
            default:
                break;
        }
    }

    [SerializeField]
    private uint energySupplyAmount = 0;
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

    [Header("-Core")]
    [Header("-Upgrade Unit Cost")]
    [SerializeField]
    private uint upgradeUnitHp = 50;
    [SerializeField]
    private uint upgradeUnitDmg = 50;

    [Header("-Upgrade Structure Cost")]
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

    [Header("-Upgrade ETC Cost")]
    [SerializeField]
    private uint upgradeEnergySupply = 100;
    [SerializeField]
    private uint upgradeMaxPopulation = 100;
}