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
    }

    public void InUnit(FriendlyObject _curObj)
    {
        if (IsBunkerFull()) return;

        if (_curObj == null) return;

        if (_curObj.ObjectType.Equals(ESelectableObjectType.UNIT_HERO)) return;

        _curObj.Position = warpPos;
        _curObj.transform.parent = transform;
        _curObj.Hold();
        //curObj.SetLayer(1 << LayerMask.GetMask("UnitInBunker"));
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
        // �� ������ ��� �� walkable Ž��
        // �θ� ����
        // ��� ��忡 �� �ڽ� ��ġ �̵�
        FriendlyObject unitObj = queueUnitInBunker.Dequeue();
        unitObj.transform.parent = null;
        unitObj.Position = SelectableObjectManager.ResetPosition(transform.position);
        // ���̾�, ���ݷ�, ���ݹ��� ����
        unitObj.ResetAttackDmg();
        unitObj.ResetAttackRange();
        unitObj.ResetLayer();
        unitObj.UpdateCurNode();
        // �� ��ġ walkable false�� ����
        grid.UpdateNodeWalkable(grid.GetNodeFromWorldPoint(transform.position), false);
        // curUnitCnt ����
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

    private BunkerInTrigger trigger = null;
    private Vector3 warpPos = Vector3.zero;


    private Queue<FriendlyObject> queueUnitInBunker = new Queue<FriendlyObject>();
}
