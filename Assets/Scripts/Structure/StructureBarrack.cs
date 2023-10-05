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
        arrMemoryPool = new MemoryPool[arrUnitPrefab.Length];

        upgradeHpCmd = new CommandUpgradeStructureHP(GetComponent<StatusHp>());

        for (int i = 0; i < arrUnitPrefab.Length; ++i)
            arrMemoryPool[i] = new MemoryPool(arrUnitPrefab[i], 3, transform);

        Subscribe();
    }

    public bool IsProcessingSpawnUnit => isProcessingSpawnUnit;

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
        return listUnit.Count < 5;
    }

    public void SpawnUnit(EUnitType _unitType)
    {
        listUnit.Add(_unitType);
        RequestSpawnUnit();
        // ui에 나타내는 내용
    }

    public override void DeactivateUnit(GameObject _removeGo, EUnitType _type)
    {
        arrMemoryPool[(int)_type].DeactivatePoolItem(_removeGo);
    }

    private void RequestSpawnUnit()
    {
        if (!isProcessingSpawnUnit && listUnit.Count > 0)
        {
            isProcessingSpawnUnit = true;
            EUnitType unitType = listUnit[0];
            StartCoroutine("SpawnUnitCoroutine", unitType);
        }
        else if(listUnit.Count < 1)
        {
            ArrayHUDCommand.Use(EHUDCommand.DISPLAY_SINGLE_INFO);
        }
    }

    private IEnumerator SpawnUnitCoroutine(EUnitType _unitType)
    {
        float elapsedTime = 0f;
        float spawnUnitDelay = arrSpawnUnitDelay[(int)_unitType];

        while (elapsedTime < spawnUnitDelay)
        {
            ArrayHUDCommand.Use(EHUDCommand.UPDATE_SPAWN_UNIT_PROGRESS, elapsedTime / spawnUnitDelay);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }

        while (!canProcessSpawnUnit)
            yield return new WaitForSeconds(1f);

        isProcessingSpawnUnit = false;
        FriendlyObject tempObj = arrMemoryPool[(int)_unitType].ActivatePoolItem(spawnPoint, 3, transform).GetComponent<FriendlyObject>();
        tempObj.Position = SelectableObjectManager.ResetPosition(tempObj.Position);
        tempObj.Init();
        tempObj.Init(myIdx);

        if (!rallyPoint.Equals(spawnPoint))
            tempObj.MoveByPos(rallyPoint);
        else if (rallyTr != null)
            tempObj.FollowTarget(rallyTr);

        listUnit.RemoveAt(0);
        ArrayPopulationCommand.Use(EPopulationCommand.INCREASE_CUR_POPULATION, _unitType);
        ArrayHUDCommand.Use(EHUDCommand.FINISH_SPAWN_UNIT);
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
        StartCoroutine("UpgradeUnitCoroutine", _upgradeType);
    }

    private IEnumerator UpgradeUnitCoroutine(EUnitUpgradeType _upgradeType)
    {
        isProcessingUpgrade = true;
        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.UNIT, _upgradeType);

        float buildFinishTime = Time.time + SelectableObjectManager.DelayUnitUpgrade;
        while (buildFinishTime > Time.time)
        {
            // ui 표시
            yield return new WaitForSeconds(0.5f);
        }
        isProcessingUpgrade = false;

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

        ArrayHUDUpgradeCommand.Use(EHUDUpgradeCommand.FINISH);
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

    private CommandUpgradeStructureHP upgradeHpCmd = null;

    private Vector3 spawnPoint = Vector3.zero;
    private Vector3 rallyPoint = Vector3.zero;
    private Transform rallyTr = null;
    private List<EUnitType> listUnit = null;

    private MemoryPool[] arrMemoryPool = null;
}
