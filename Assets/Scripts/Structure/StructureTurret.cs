using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTurret : Structure
{
    public override void Init(int _structureIdx)
    {
        base.Init(_structureIdx);
        selectObj = GetComponent<FriendlyObject>();
        selectObj.Init();
        selectObj.SetMyTr(turretHeadTr);
        myIdx = _structureIdx;
    }

    public override void BuildComplete()
    {
        base.BuildComplete();
        GetComponent<FriendlyObject>().Hold();
    }

    public override void Upgrade()
    {
        if(upgradeLevel < StructureManager.UpgradeLimit)
            StartCoroutine("UpgradeCoroutine");
    }

    private IEnumerator UpgradeCoroutine()
    {
        float buildFinishTime = Time.time + upgradeDelay;
        while (buildFinishTime > Time.time)
        {
            // ui ǥ��
            yield return new WaitForSeconds(0.5f);
        }

        ++upgradeLevel;
        // ���⼭ �� �湮�� ����? �ϱ�
    }

    [SerializeField]
    private Transform turretHeadTr = null;
    [SerializeField]
    private float upgradeDelay = 0f;

    private FriendlyObject selectObj = null;
}
