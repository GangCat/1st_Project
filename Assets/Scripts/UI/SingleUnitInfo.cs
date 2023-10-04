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
        imageProgressbar = GetComponentInChildren<ImageProgressbarAutoController>();

        textInfoUnitName.Init();
        imageProgressbar.Init();
        
        SetActive(false);
    }

    public void InitContainer(UnitInfoContainer _container)
    {
        container = _container;
        statInfo.Init(container);
        imageProgressbar.Init(container);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void DisplaySingleInfo()
    {
        SetActive(true);
        textInfoUnitName.UpdateText(Enum.GetName(typeof(EObjectType), container.objectType));
        statInfo.DisplayInfo();
        imageProgressbar.UpdateHp();
    }

    public void HideDisplay()
    {
        imageProgressbar.StopUpdateHp();
        SetActive(false);
    }

    private StatusInfo statInfo = null;
    private TextInfoBase textInfoUnitName = null;
    private UnitInfoContainer container = null;
    private ImageProgressbarAutoController imageProgressbar = null;
}
