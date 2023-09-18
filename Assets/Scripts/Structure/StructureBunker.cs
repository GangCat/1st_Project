using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureBunker : Structure
{
    public override void Init()
    {
        trigger = GetComponentInChildren<BunkerInTrigger>();
        warpPos = transform.position;
        warpPos.y += height;
        trigger.Init(capacity);
    }

    public SelectableObject InUnit()
    {
        SelectableObject curObj = trigger.GetCurObj();

        if (curObj == null) return null;

        if (curObj.ObjectType.Equals(ESelectableObjectType.UNIT_HERO)) return null;

        curObj.Position = warpPos;
        curObj.transform.parent = transform;
        curObj.Hold();
        //curObj.SetLayer(1 << LayerMask.GetMask("UnitInBunker"));
        curObj.SetLayer(LayerMask.NameToLayer("UnitInBunker"));
        curObj.SetAttackDmg(buffRatio);
        curObj.SetAttackRange(buffRatio);
        curObj.UpdateCurNode();
        curObj.IsBunker = false;
        ++curUnitCnt;
        trigger.ResetObj();
        trigger.UpdateUnitCnt(curUnitCnt);
        return curObj;
    }

    public void OutOneUnit()
    {
        // �� ������ ��� �� walkable Ž��
        // �θ� ����
        // ��� ��忡 �� �ڽ� ��ġ �̵�
        SelectableObject[] obj = GetComponentsInChildren<SelectableObject>();
        if (obj.Length > 1)
        {
            SelectableObject unitObj = obj[1];
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
            --curUnitCnt;
            trigger.UpdateUnitCnt(curUnitCnt);
        }
    }

    public void OutAllUnit()
    {
        StartCoroutine("OutAllUnitCoroutine");
    }

    private IEnumerator OutAllUnitCoroutine()
    {
        while (curUnitCnt > 0)
        {
            OutOneUnit();
            yield return new WaitForSeconds(1f);
        }
    }

    [SerializeField, Range(0, 1)]
    private float buffRatio = 0f;
    [SerializeField]
    private float height = 5f;

    private BunkerInTrigger trigger = null;
    private Vector3 warpPos = Vector3.zero;

    private int capacity = 1;
    private int curUnitCnt = 0;
}
