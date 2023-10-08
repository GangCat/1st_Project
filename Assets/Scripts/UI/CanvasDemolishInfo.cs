using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDemolishInfo : MonoBehaviour
{
    public void Init()
    {
        imageUnit = GetComponentInChildren<ImageModel>();
        textInfoStructureName = GetComponentInChildren<TextBase>();
        imageUnit.Init();
        textInfoStructureName.Init();
        imageProgressbar.Init();
        SetActive(false);
    }

    public void ShowDisplay()
    {
        SetActive(true);
    }

    public void HideDisplay()
    {
        SetActive(false);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void UpdateDemolishTime(float _percent)
    {
        imageProgressbar.UpdateLength(_percent);
    }

    public void UpdateUnit(EObjectType _type)
    {
        imageUnit.ChangeSprite(arrSpriteObject[(int)_type - 3]);
        textInfoStructureName.UpdateText(Enum.GetName(typeof(EObjectType), _type));
    }


    [SerializeField]
    private ImageProgressbar imageProgressbar = null;

    [Header("-TURRET, BUNKER, WALL, BARRACK, NUCLEAR")]
    [SerializeField]
    private Sprite[] arrSpriteObject = null;

    private ImageModel imageUnit = null;
    private TextBase textInfoStructureName = null;
}
