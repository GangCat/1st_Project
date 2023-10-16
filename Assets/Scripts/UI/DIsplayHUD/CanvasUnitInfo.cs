using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUnitInfo : MonoBehaviour
{
    public void Init()
    {
        groupInfo = GetComponentInChildren<GroupUnitInfo>();
        singleInfo = GetComponentInChildren<SingleUnitInfo>();

        groupInfo.Init();
        singleInfo.Init();
    }

    public void InitGroupUnitInfo(List<SFriendlyUnitInfo> _list)
    {
        groupInfo.InitList(_list);
    }

    public void InitSingleUnitInfo(UnitInfoContainer _container)
    {
        singleInfo.InitContainer(_container);
    }

    public void DisplaySingleUnitInfo()
    {
        groupInfo.HideDisplay();
        singleInfo.DisplaySingleInfo();
    }

    public void DisplayGroupUnitInfo(int _unitCnt)
    {
        singleInfo.HideDisplay();
        groupInfo.DisplayGroupInfo(_unitCnt);
    }

    public void HideDisplay()
    {
        singleInfo.HideDisplay();
        groupInfo.HideDisplay();
    }


    private GroupUnitInfo groupInfo = null;
    private SingleUnitInfo singleInfo = null;
}
