using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUnitInfo : MonoBehaviour
{
    public void Init()
    {
        statInfo = GetComponentInChildren<StatusInfo>();
        textInfoUnitName = GetComponentInChildren<TextBase>();
        imageProgressbar = GetComponentInChildren<ImageProgressbarAutoController>();
        imageModel = GetComponentInChildren<ImageModel>();

        textInfoUnitName.Init();
        imageProgressbar.Init();
        imageModel.Init();

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

        if (container.unitType.Equals(EUnitType.NONE))
            imageModel.ChangeSprite(arrOtherSprite[(int)container.objectType - 2]);
        else
            imageModel.ChangeSprite(arrFriendlyUnitSprite[(int)container.unitType]);
    }

    public void HideDisplay()
    {
        imageProgressbar.StopUpdateHp();
        SetActive(false);
    }

    [SerializeField]
    private Sprite[] arrOtherSprite = null;
    [SerializeField]
    private Sprite[] arrFriendlyUnitSprite = null;

    private StatusInfo statInfo = null;
    private TextBase textInfoUnitName = null;
    private UnitInfoContainer container = null;
    private ImageProgressbarAutoController imageProgressbar = null;
    private ImageModel imageModel = null;
}
