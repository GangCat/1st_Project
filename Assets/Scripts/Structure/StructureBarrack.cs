using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBarrack : Structure, ISubscriber
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        spawnPoint = transform.position;
        rallyPoint = spawnPoint;
        listUnit = new List<EUnitType>();
        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());

        Subscribe();
    }

    public bool IsProcessingSpawnUnit => isProcessingSpawnUnit;

    public override void CancleCurAction()
    {
        if (isProcessingUpgrade)
        {
            if (isProcessingUpgradeUnit)
            {
                StopCoroutine("UpgradeUnitCoroutine");
                isProcessingUpgrade = false;
                isProcessingUpgradeUnit = false;
                curUpgradeType = EUpgradeType.NONE;
            }
            else
            {
                StopCoroutine("UpgradeCoroutine");
                isProcessingUpgrade = false;
                curUpgradeType = EUpgradeType.NONE;
            }
        }
        else if (isProcessingConstruct)
        {
            StopCoroutine("BuildStructureCoroutine");
            isProcessingConstruct = false;
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH_COMPLETE, myStructureIdx);
            DestroyStructure();
        }
        else if (isProcessingDemolish)
        {
            StopCoroutine("DemolishCoroutine");
            isProcessingDemolish = false;
        }
        else if (isProcessingSpawnUnit)
        {
            StopCoroutine("SpawnUnitCoroutine");
            listUnit.RemoveAt(0);
            isProcessingSpawnUnit = false;
            RequestSpawnUnit();
        }

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void UpdateSpawnInfo()
    {
        ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_LIST, listUnit);
        ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_TIME, SpawnUnitProgressPercent);
    }

    protected override void UpgradeComplete()
    {
        base.UpgradeComplete();
        Debug.Log("BarrackUpgradeComplete");
        upgradeHpCmd.Execute(upgradeHpAmount);
    }

    public void SetRallyPoint(Vector3 _rallyPoint)
    {
        rallyTr = null;
        rallyPoint = _rallyPoint;
    }

    public void SetRallyPoint(Transform _rallyTr)
    {
        rallyPoint = spawnPoint;
        rallyTr = _rallyTr;
    }

    public bool CanSpawnUnit()
    {
        if(isProcessingConstruct)
            return false;

        return listUnit.Count < 5;
    }

    public void StartSpawnUnit(EUnitType _unitType)
    {
        listUnit.Add(_unitType);
        if (myObj.IsSelect)
            ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_LIST, listUnit);
        RequestSpawnUnit();
        // ui에 나타내는 내용
    }

    public override void Demolish()
    {
        if (isProcessingSpawnUnit) return;
        base.Demolish();
    }

    private void RequestSpawnUnit()
    {
        if (listUnit.Count < 1 && myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        else if (!isProcessingSpawnUnit && listUnit.Count > 0)
        {
            EUnitType unitType = listUnit[0];
            StartCoroutine("SpawnUnitCoroutine", unitType);
        }
    }

    private IEnumerator SpawnUnitCoroutine(EUnitType _unitType)
    {
        isProcessingSpawnUnit = true;
        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
        float elapsedTime = 0f;
        float spawnUnitDelay = arrSpawnUnitDelay[(int)_unitType];
        SpawnUnitProgressPercent = elapsedTime / spawnUnitDelay;

        while (SpawnUnitProgressPercent < 1)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_TIME, SpawnUnitProgressPercent);
            yield return new WaitForSeconds(0.5f);

            elapsedTime += 0.5f;
            SpawnUnitProgressPercent = elapsedTime / spawnUnitDelay;
        }

        while (!canProcessSpawnUnit)
            yield return new WaitForSeconds(1f);

        isProcessingSpawnUnit = false;
        listUnit.RemoveAt(0);
        ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_SPAWN_UNIT, _unitType, spawnPoint, rallyPoint, rallyTr);
        ArrayPopulationCommand.Use(EPopulationCommand.INCREASE_CUR_POPULATION, _unitType);
        if (myObj.IsSelect)
            ArrayHUDSpawnUnitCommand.Use(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_LIST, listUnit);
        RequestSpawnUnit();
    }

    public bool CanUpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                if (!isProcessingUpgrade && SelectableObjectManager.LevelRangedUnitDmgUpgrade < upgradeLevel << 1)
                    return true;
                return false;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                if (!isProcessingUpgrade && SelectableObjectManager.LevelRangedUnitHpUpgrade < upgradeLevel << 1)
                    return true;
                return false;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                if (!isProcessingUpgrade && SelectableObjectManager.LevelMeleeUnitDmgUpgrade < upgradeLevel << 1)
                    return true;
                return false;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                if (!isProcessingUpgrade && SelectableObjectManager.LevelMeleeUnitHpUpgrade < upgradeLevel << 1)
                    return true;
                return false;
            default:
                return false;
        }
    }

    public void UpgradeUnit(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                curUpgradeType = EUpgradeType.RANGED_DMG;
                break;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                curUpgradeType = EUpgradeType.RANGED_HP;
                break;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                curUpgradeType = EUpgradeType.MELEE_DMG;
                break;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                curUpgradeType = EUpgradeType.MELEE_HP;
                break;
            default:
                break;
        }

        StartCoroutine("UpgradeUnitCoroutine", _upgradeType);
    }

    private IEnumerator UpgradeUnitCoroutine(EUnitUpgradeType _upgradeType)
    {
        isProcessingUpgrade = true;
        isProcessingUpgradeUnit = true;

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);

        float elapsedTime = 0f;
        progressPercent = elapsedTime / upgradeDelay;
        while (elapsedTime < upgradeDelay)
        {
            while (isPause)
                yield return null;

            if (myObj.IsSelect)
                ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UPDATE_UPGRADE_TIME, progressPercent);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
            progressPercent = elapsedTime / upgradeDelay;
        }

        isProcessingUpgrade = false;
        isProcessingUpgradeUnit = false;
        UpgradeUnitComplete(_upgradeType);
    }

    private void UpgradeUnitComplete(EUnitUpgradeType _upgradeType)
    {
        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UNIT_DMG:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_DMG);
                break;
            case EUnitUpgradeType.RANGED_UNIT_HP:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_HP);
                break;
            case EUnitUpgradeType.MELEE_UNIT_DMG:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_DMG);
                break;
            case EUnitUpgradeType.MELEE_UNIT_HP:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_HP);
                break;
        }

        if (myObj.IsSelect)
            ArrayUICommand.Use(EUICommand.UPDATE_INFO_UI);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.POPULATION_MANAGER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        switch (_message)
        {
            case EMessageType.START_SPAWN:
                canProcessSpawnUnit = true;
                break;
            case EMessageType.STOP_SPAWN:
                canProcessSpawnUnit = false;
                break;
            default:
                break;
        }
    }

    [Header("-Melee, Range, Rocket(temp)")]
    [SerializeField]
    private float[] arrSpawnUnitDelay = null;
    [SerializeField]
    private GameObject[] arrUnitPrefab = null;

    [Header("-Upgrade Attribute")]
    [SerializeField]
    private float upgradeHpAmount = 0f;

    private bool isProcessingSpawnUnit = false;
    private bool canProcessSpawnUnit = true;
    private bool isProcessingUpgradeUnit = false;

    private CommandUpgradeStructureHP upgradeHpCmd = null;

    private Vector3 spawnPoint = Vector3.zero;
    private Vector3 rallyPoint = Vector3.zero;
    private Transform rallyTr = null;
    private List<EUnitType> listUnit = null;

    private float SpawnUnitProgressPercent = 0f;
}
