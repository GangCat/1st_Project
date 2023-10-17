using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IPublisher, IPauseObserver
{
    public void Init()
    {
        ArrayPauseCommand.Use(EPauseCommand.REGIST, this);
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_ENERGY_HUD, curEnergy);
        ArrayCurrencyCommand.Use(ECurrencyCommand.UPDATE_CORE_HUD, curCore);
        StartCoroutine("SupplyEnergyCoroutine");
    }

    public static uint UpgradeCost(EObjectType _type)
    {
        switch (_type)
        {
            case EObjectType.MAIN_BASE:
                return upgradeMainBaseCost;
            case EObjectType.TURRET:
                return upgradeTurretCost;
            case EObjectType.BUNKER:
                return upgradeBunkerCost;
            case EObjectType.WALL:
                return upgradeWallCost;
            case EObjectType.BARRACK:
                return upgradeBarrackCost;
            default:
                return 0;
        }
    }

    public static uint UpgradeUnitCost(EUnitUpgradeType _type)
    {
        switch (_type)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                return upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                return upgradeUnitHpCost * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                return upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                return upgradeUnitHpCost * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade;
            default:
                return 0;
        }
    }

    public static uint UpgradeETCCost(EUpgradeETCType _type)
    {
        switch (_type)
        {
            case EUpgradeETCType.CURRENT_MAX_POPULATION:
                return upgradeMaxPopulation;
            case EUpgradeETCType.ENERGY_SUPPLY:
                return upgradeEnergySupply;
            default:
                return 0;
        }
    }

    private IEnumerator SupplyEnergyCoroutine()
    {
        float energySupplyDelay = 0f;

        while (true)
        {
            while (isPause)
                yield return null;

            if (energySupplyDelay >= energySupplyRate)
            {
                curEnergy = Functions.ClampMaxWithUInt(curEnergy + energySupplyAmount, maxEnergy);
                UpdateEnergy();
                energySupplyDelay = 0f;
            }

            yield return new WaitForSeconds(1f);
            energySupplyDelay += 1f;
        }
    }

    private void DecreaseEnergy(uint _decreaseEnergy)
    {
        curEnergy -= _decreaseEnergy;
        UpdateEnergy();
    }

    private void IncreaseEnergy(uint _increaseEnergy)
    {
        curEnergy = Functions.ClampMaxWithUInt(maxEnergy, curEnergy + _increaseEnergy);
        UpdateEnergy();
    }

    private void DecreaseCore(uint _decreaseCore)
    {
        curCore -= _decreaseCore;
        UpdateCore();
    }

    public void IncreaseCore(uint _increaseCore)
    {
        curCore = Functions.ClampMaxWithUInt(curCore + _increaseCore, maxCore);
        UpdateCore();
    }

    private bool IsCoreEnough(uint _decreaseCore)
    {
        if (curCore < _decreaseCore)
        {
            // Not enough core Audio Play
            audioType = EAudioType_Advisor.CORE;
            AudioManager.instance.PlayAudio_Advisor(audioType);
            return false;
        }
        return true;
    }

    private bool IsEnergyEnough(uint _decreaseEnergy)
    {
        if (curEnergy < _decreaseEnergy)
        {
            // Not enough energy Audio Play
            audioType = EAudioType_Advisor.ENERGY;
            AudioManager.instance.PlayAudio_Advisor(audioType);
            
            return false;
        }
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

    #region BuildStructure
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

    public void CancleBuildStructure(EObjectType _objType)
    {
        switch (_objType)
        {
            case EObjectType.TURRET:
                IncreaseEnergy(buildTurret);
                break;
            case EObjectType.BUNKER:
                IncreaseEnergy(buildBunker);
                break;
            case EObjectType.WALL:
                IncreaseEnergy(buildWall);
                break;
            case EObjectType.BARRACK:
                IncreaseEnergy(buildBarrack);
                break;
            case EObjectType.NUCLEAR:
                IncreaseEnergy(buildNuclear);
                break;
            default:
                break;
        }
    }
    #endregion

    #region UpgradeStructure
    public bool CanUpgradeSturcture(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                return IsCoreEnough(upgradeMainBaseCost * (uint)_level);
            case EObjectType.TURRET:
                return IsCoreEnough(upgradeTurretCost * (uint)_level);
            case EObjectType.BUNKER:
                return IsCoreEnough(upgradeBunkerCost * (uint)_level);
            case EObjectType.WALL:
                return IsCoreEnough(upgradeWallCost * (uint)_level);
            case EObjectType.BARRACK:
                return IsCoreEnough(upgradeBarrackCost * (uint)_level);
            default:
                return false;
        }
    }

    public void UpgradeStructure(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                DecreaseCore(upgradeMainBaseCost * (uint)_level);
                break;
            case EObjectType.TURRET:
                DecreaseCore(upgradeTurretCost * (uint)_level);
                break;
            case EObjectType.BUNKER:
                DecreaseCore(upgradeBunkerCost * (uint)_level);
                break;
            case EObjectType.WALL:
                DecreaseCore(upgradeWallCost * (uint)_level);
                break;
            case EObjectType.BARRACK:
                DecreaseCore(upgradeBarrackCost * (uint)_level);
                break;
            default:
                break;
        }
    }

    public void CancleUpgradeStructure(EObjectType _objType, int _level)
    {
        switch (_objType)
        {
            case EObjectType.MAIN_BASE:
                IncreaseCore(upgradeMainBaseCost * (uint)_level);
                break;
            case EObjectType.TURRET:
                IncreaseCore(upgradeTurretCost * (uint)_level);
                break;
            case EObjectType.BUNKER:
                IncreaseCore(upgradeBunkerCost * (uint)_level);
                break;
            case EObjectType.WALL:
                IncreaseCore(upgradeWallCost * (uint)_level);
                break;
            case EObjectType.BARRACK:
                IncreaseCore(upgradeBarrackCost * (uint)_level);
                break;
            default:
                break;
        }
    }
    #endregion

    #region UpgradeUnit
    public bool CanUpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                return IsCoreEnough(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
            case EUnitUpgradeType.RANGED_UNIT_HP:
                return IsCoreEnough(upgradeUnitHpCost * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                return IsCoreEnough(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
            case EUnitUpgradeType.MELEE_UNIT_HP:
                return IsCoreEnough(upgradeUnitHpCost * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
            default:
                return false;
        }
    }

    public void UpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                DecreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                DecreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                DecreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                DecreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
                break;
            default:
                break;
        }
    }

    public void CancleUpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                IncreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                IncreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelRangedUnitHpUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                IncreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
                break;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                IncreaseCore(upgradeUnitDmgCost * (uint)SelectableObjectManager.LevelMeleeUnitHpUpgrade);
                break;
            default:
                break;
        }
    }
    #endregion

    #region SpawnUnit
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

    public void CancleSpawnUnit(EUnitType _unitType)
    {
        switch (_unitType)
        {
            case EUnitType.MELEE:
                IncreaseEnergy(spawnMeleeUnit);
                break;
            case EUnitType.RANGED:
                IncreaseEnergy(spawnRangedUnit);
                break;
            default:
                break;
        }
    }
    #endregion

    #region UpgradeETC
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

    public void CancleUpgradeETC(EUpgradeETCType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUpgradeETCType.CURRENT_MAX_POPULATION:
                IncreaseCore(upgradeMaxPopulation);
                break;
            case EUpgradeETCType.ENERGY_SUPPLY:
                IncreaseCore(upgradeEnergySupply);
                break;
            default:
                break;
        }
    }
    #endregion

    #region SpawnNuclear
    public bool CanSpawnNuclear()
    {
        return curEnergy > spawnNuclear;
    }

    public void SpawnNuclear()
    {
        DecreaseEnergy(spawnNuclear);
    }

    public void CancleSpawnNuclear()
    {
        IncreaseEnergy(spawnNuclear);
    }

    #endregion

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
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

    [Header("-Spawn Cost")]
    [SerializeField]
    private uint spawnMeleeUnit = 70;
    [SerializeField]
    private uint spawnRangedUnit = 50;
    [SerializeField]
    private uint spawnNuclear = 1000;

    [Header("-Core")]
    [Header("-Upgrade Unit Cost")]
    [SerializeField]
    private static uint upgradeUnitHpCost = 50;
    [SerializeField]
    private static uint upgradeUnitDmgCost = 50;

    [Header("-Upgrade Structure Cost")]
    [SerializeField]
    private static uint upgradeMainBaseCost = 200;
    [SerializeField]
    private static uint upgradeBarrackCost = 100;
    [SerializeField]
    private static uint upgradeBunkerCost = 70;
    [SerializeField]
    private static uint upgradeWallCost = 30;
    [SerializeField]
    private static uint upgradeTurretCost = 100;

    [Header("-Upgrade ETC Cost")]
    [SerializeField]
    private static uint upgradeEnergySupply = 100;
    [SerializeField]
    private static uint upgradeMaxPopulation = 100;

    private bool isPause = false;
    
    private EAudioType_Advisor audioType;
}