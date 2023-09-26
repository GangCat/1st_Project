using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureBunker : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        trigger = GetComponentInChildren<BunkerInTrigger>();
        warpPos = transform.position;
        warpPos.y += height;
        trigger.Init(transform);
        myIdx = _structureIdx;
        upgradeHpCmd = new CommandUpgradeHP(GetComponent<StatusHp>());
        upgradeBuffRatioCmd = new CommandUpgradeBuffRatio(this);
    }

    protected override void UpgradeComplete()
    {
        base.UpgradeComplete();
        upgradeHpCmd.Execute(increaseHpAmount);
        upgradeBuffRatioCmd.Execute();
        Debug.Log("UpgradeCompleteBunker");
    }

    public void UpgradeBuffRatio()
    {
        buffRatio += increaseBuffRatioAmount;
        if (IsBunkerFull())
        {
            FriendlyObject tempObj = queueUnitInBunker.Peek();
            tempObj.SetAttackDmg(buffRatio);
            tempObj.SetAttackRange(buffRatio);
        }
    }

    public void InUnit(FriendlyObject _curObj)
    {
        if (IsBunkerFull()) return;

        if (_curObj == null) return;

        if (_curObj.GetObjectType().Equals(EObjectType.UNIT_HERO)) return;

        _curObj.Position = warpPos;
        _curObj.transform.parent = transform;
        _curObj.Hold();
        _curObj.SetLayer(LayerMask.NameToLayer("UnitInBunker"));
        _curObj.SetAttackDmg(buffRatio);
        _curObj.SetAttackRange(buffRatio);
        _curObj.UpdateCurNode();
        queueUnitInBunker.Enqueue(_curObj);
        trigger.ResetObj();
    }

    public void OutOneUnit()
    {
        if (queueUnitInBunker.Count < 1) return;
        // 내 주위의 노드 중 walkable 탐색
        // 부모 변경
        // 결과 노드에 내 자식 위치 이동
        FriendlyObject unitObj = queueUnitInBunker.Dequeue();
        unitObj.transform.parent = null;
        unitObj.Position = SelectableObjectManager.ResetPosition(transform.position);
        // 레이어, 공격력, 공격범위 리셋
        unitObj.ResetAttackDmg();
        unitObj.ResetAttackRange();
        unitObj.ResetLayer();
        unitObj.UpdateCurNode();
        // 내 위치 walkable false로 변경
        grid.UpdateNodeWalkable(grid.GetNodeFromWorldPoint(transform.position), false);
    }

    public void OutAllUnit()
    {
        StartCoroutine("OutAllUnitCoroutine");
    }

    private IEnumerator OutAllUnitCoroutine()
    {
        while (queueUnitInBunker.Count() > 0)
        {
            OutOneUnit();
            yield return new WaitForSeconds(1f);
        }
    }

    private bool IsBunkerFull()
    {
        return queueUnitInBunker.Count() >= capacity;
    }

    [SerializeField, Range(0, 1)]
    private float buffRatio = 0f;
    [SerializeField]
    private float height = 5f;
    [SerializeField]
    private int capacity = 1;
    [SerializeField]
    private float increaseBuffRatioAmount = 0f;
    [SerializeField]
    private float increaseHpAmount = 0f;

    private BunkerInTrigger trigger = null;
    private Vector3 warpPos = Vector3.zero;
    private CommandUpgradeHP upgradeHpCmd = null;
    private CommandUpgradeBuffRatio upgradeBuffRatioCmd = null;

    private Queue<FriendlyObject> queueUnitInBunker = new Queue<FriendlyObject>();
}
