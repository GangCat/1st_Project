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
        groupInfo.SetActive(false);
        singleInfo.DisplaySingleInfo();
    }

    public void DisplayGroupUnitInfo(int _unitCnt)
    {
        singleInfo.SetActive(false);
        groupInfo.DisplayGroupInfo(_unitCnt);
    }

    public void HideDisplay()
    {
        singleInfo.SetActive(false);
        groupInfo.SetActive(false);
    }


    private GroupUnitInfo groupInfo = null;
    private SingleUnitInfo singleInfo = null;
}
