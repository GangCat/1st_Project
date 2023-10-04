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

    private IEnumerator UpdateHpDisplayCoroutine()
    {
        Color color = Color.white;
        while (true)
        {
            color.g = unitInfo;
            color.b = unitInfo;
            myImage.color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void updateHpDisplay(float _hpPercent)
    {
        oriColor.g = _hpPercent;
        oriColor.b = _hpPercent;
        myImage.color = oriColor;
    }

    public void SetActive(bool _isActive)
    {
        if (_isActive)
        {
            gameObject.SetActive(true);
            //StartCoroutine("UpdateHpDisplayCoroutine");
        }
        else
        {
            //StopAllCoroutines();
            gameObject.SetActive(false);
        }

    }

    public void ChangeSprite(Sprite _sprite)
    {
        myImage.sprite = _sprite;
    }

    private Color oriColor;
    private Image myImage = null;
    private float unitInfo;
}
