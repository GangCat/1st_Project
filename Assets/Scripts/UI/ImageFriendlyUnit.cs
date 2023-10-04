using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFriendlyUnit : MonoBehaviour
{
    public void Init(ref float _hpPercent)
    {
        myImage = GetComponent<Image>();
        SetActive(false);
        unitInfo = _hpPercent;
        oriColor = Color.white;
    }

    public void updateHpDisplay(float _hpPercent)
    {
        oriColor.g = _hpPercent;
        oriColor.b = _hpPercent;
        myImage.color = oriColor;
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void ChangeSprite(Sprite _sprite)
    {
        myImage.sprite = _sprite;
    }

    private Color oriColor;
    private Image myImage = null;
    private float unitInfo;
}
