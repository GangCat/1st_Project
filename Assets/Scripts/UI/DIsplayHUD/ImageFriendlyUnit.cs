using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFriendlyUnit : ImageModel
{
    public override void Init()
    {
        myImage = GetComponent<Image>();
        SetActive(false);
        oriColor = Color.white;
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void updateHpDisplay(float _hpPercent)
    {
        oriColor.g = _hpPercent;
        oriColor.b = _hpPercent;
        myImage.color = oriColor;
    }

    private Color oriColor;
}
