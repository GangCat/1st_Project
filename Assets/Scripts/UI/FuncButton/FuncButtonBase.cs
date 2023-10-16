using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class FuncButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public abstract void Init();

    public void OnPointerEnter(PointerEventData eventData)
    {
        ArrayHUDCommand.Use(EHUDCommand.DISPLAY_TOOLTIP, funcName, funcHotkey, cost, currencyType, GetComponent<RectTransform>().position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ArrayHUDCommand.Use(EHUDCommand.HIDE_TOOLTIP);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void SetCost(int _cost)
    {
        cost = _cost.ToString();
    }

    public void SetHotkey(KeyCode _hotkey)
    {
        funcHotkey = Enum.GetName(typeof(KeyCode), _hotkey);
    }

    [SerializeField]
    protected string funcName = null;
    [SerializeField]
    protected string funcHotkey = null;
    [SerializeField]
    protected string cost = null;
    [SerializeField]
    protected ECurrencyType currencyType = ECurrencyType.NONE;
}
