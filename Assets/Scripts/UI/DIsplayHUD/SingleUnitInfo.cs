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
        textStructureLevel = GetComponentInChildren<TextStructureLevel>();
        imageProgressbar = GetComponentInChildren<ImageProgressbarAutoController>();
        imageModel = GetComponentInChildren<ImageModel>();

        textInfoUnitName.Init();
        textInfoUnitDescription.Init();
        textStructureLevel.Init();
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

    public void DisplaySingleInfo(string _objectName, string _objectDescription, int _structureLevel = 0)
    {
        SetActive(true);
        textInfoUnitName.UpdateText(_objectName);
        textInfoUnitDescription.UpdateText(_objectDescription);
        if (!_structureLevel.Equals(0))
        {
            textStructureLevel.SetActive(true);
            textStructureLevel.UpdateText(_structureLevel.ToString());
        }
        else
            textStructureLevel.SetActive(false);

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
    private TextStructureLevel textStructureLevel = null;
    private UnitInfoContainer container = null;
    private ImageProgressbarAutoController imageProgressbar = null;
    private ImageModel imageModel = null;
}
