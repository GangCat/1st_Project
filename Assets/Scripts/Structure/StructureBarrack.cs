using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBarrack : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        spawnPoint = transform.position;
        rallyPoint = spawnPoint;
        arrMemoryPool = new MemoryPool[arrUnitPrefab.Length];
        upgradeHpCmd = new CommandUpgradeHP(GetComponent<StatusHp>());

        for (int i = 0; i < arrUnitPrefab.Length; ++i)
            arrMemoryPool[i] = new MemoryPool(arrUnitPrefab[i], 3, transform);
    }

    public int UpgradeLevel => upgradeLevel;

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

    public void SpawnUnit(ESpawnUnitType _unitType)
    {
        if (listUnit.Count >= 5) return;

        listUnit.Add(_unitType);
        RequestSpawnUnit();
        // ui에 나타내는 내용
    }

    public override void DeactivateUnit(GameObject _removeGo, ESpawnUnitType _type)
    {
        arrMemoryPool[(int)_type].DeactivatePoolItem(_removeGo);
    }

    private void RequestSpawnUnit()
    {
        if (!isProcessingSpawnUnit && listUnit.Count > 0)
        {
            isProcessingSpawnUnit = true;
            ESpawnUnitType unitType = listUnit[0];
            StartCoroutine("SpawnUnitCoroutine", unitType);
        }
    }

    private IEnumerator SpawnUnitCoroutine(ESpawnUnitType _unitType)
    {
        float elapsedTime = 0f;

        while (elapsedTime < spawnUnitDelay[(int)_unitType])
        {
            // elapsedTime으로 그 게이지 바 표시하기
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }

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
        RequestSpawnUnit();
    }

    public void UpgradeUnitDmg()
    {
        if (!isProcessingUpgrade && SelectableObjectManager.LevelRangedUnitDmgUpgrade < upgradeLevel * 2)
            StartCoroutine("UpgradeUnitCoroutine", EUnitUpgradeType.RANGED_UPGRADE_DMG);
    }

    public void UpgradeUnitHp()
    {
        if (!isProcessingUpgrade && SelectableObjectManager.LevelRangedUnitHpUpgrade < upgradeLevel * 2)
            StartCoroutine("UpgradeUnitCoroutine", EUnitUpgradeType.RANGED_UPGRADE_HP);
    }

    private IEnumerator UpgradeUnitCoroutine(EUnitUpgradeType _upgradeType)
    {
        isProcessingUpgrade = true;

        float buildFinishTime = Time.time + SelectableObjectManager.DelayUnitUpgrade;
        while (buildFinishTime > Time.time)
        {
            // ui 표시
            yield return new WaitForSeconds(0.5f);
        }

        switch (_upgradeType)
        {
            case EUnitUpgradeType.RANGED_UPGRADE_DMG:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_DMG);
                break;
            case EUnitUpgradeType.RANGED_UPGRADE_HP:
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_HP);
                break;
            case EUnitUpgradeType.MELEE_UPGRADE_DMG:
                break;
            case EUnitUpgradeType.MELEE_UPGRADE_HP:
                break;
        }

        isProcessingUpgrade = false;
    }

    [Header("-Melee(temp), Range, Rocket(temp)")]
    [SerializeField]
    private float[] spawnUnitDelay = new float[(int)ESpawnUnitType.LENGTH];
    [SerializeField]
    private GameObject[] arrUnitPrefab = new GameObject[(int)ESpawnUnitType.LENGTH];

    [Header("-Upgrade Attribute")]
    [SerializeField]
    private float upgradeHpAmount = 0f;

    private bool isProcessingSpawnUnit = false;
    private bool isProcessingUpgrade = false;

    private CommandUpgradeHP upgradeHpCmd = null;

    private Vector3 spawnPoint = Vector3.zero;
    private Vector3 rallyPoint = Vector3.zero;
    private Transform rallyTr = null;
    private List<ESpawnUnitType> listUnit = new List<ESpawnUnitType>();

    private MemoryPool[] arrMemoryPool = null;
}
