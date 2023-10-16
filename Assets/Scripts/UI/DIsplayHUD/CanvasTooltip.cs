using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasTooltip : MonoBehaviour
{
    public void Init()
    {
        imageTooltip = GetComponentInChildren<ImageTooltip>();
        imageTooltip.Init();

        SetActive(false);
    }

    public void DisplayTooltip(string _funcName, string _funcHotkey, string _cost, ECurrencyType _currencyType, Vector3 funcButtonPos)
    {
        SetActive(true);
        imageTooltip.UpdateInfo(_funcName, _funcHotkey, _cost, _currencyType, funcButtonPos);
    }

    public void HideTooltip()
    {
        SetActive(false);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }


    private ImageTooltip imageTooltip = null;
}