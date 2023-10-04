using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUnitInfo : MonoBehaviour
{
    public void Init()
    {
        statInfo = GetComponentInChildren<StatusInfo>();
        textInfoUnitName = GetComponentInChildren<TextInfoBase>();

        textInfoUnitName.Init();
        SetActive(false);
    }

    public void InitContainer(UnitInfoContainer _container)
    {
        container = _container;
        statInfo.Init(container);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void DisplaySingleInfo()
    {
        textInfoUnitName.UpdateText(Enum.GetName(typeof(EObjectType), container.objectType));
        statInfo.DisplayInfo();
        SetActive(true);
    }

    private StatusInfo statInfo = null;
    private TextInfoBase textInfoUnitName = null;
    private UnitInfoContainer container = null;
}
