using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUnitInfo : MonoBehaviour
{
    public void Init()
    {
        statInfo = GetComponentInChildren<StatusInfo>();
        textInfoUnitName = GetComponentInChildren<TextInfoUnitName>();
        textInfoUnitDescription = GetComponentInChildren<TextInfoUnitDescription>();
        imageProgressbar = GetComponentInChildren<ImageProgressbarAutoController>();
        imageModel = GetComponentInChildren<ImageModel>();

        textInfoUnitName.Init();
        textInfoUnitDescription.Init();
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

    public void DisplaySingleInfo(string _objectName, string _objectDescription)
    {
        SetActive(true);
        textInfoUnitName.UpdateText(_objectName);
        textInfoUnitDescription.UpdateText(_objectDescription);
        statInfo.DisplayInfo();
        imageProgressbar.UpdateHp();

        if (container.unitType.Equals(EUnitType.NONE))
            imageModel.ChangeSprite(arrOtherSprite[(int)container.objectType - 3]);
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
    private TextInfoUnitName textInfoUnitName = null;
    private TextInfoUnitDescription textInfoUnitDescription = null;
    private UnitInfoContainer container = null;
    private ImageProgressbarAutoController imageProgressbar = null;
    private ImageModel imageModel = null;
}
