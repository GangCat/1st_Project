using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ImageTooltip : MonoBehaviour
{
    public void Init()
    {
        myRt = GetComponent<RectTransform>();
        tooltipOffset = new Vector3(-37f, 85f, 0f);
    }

    public void UpdateInfo(string _funcName, string _funcHotkey, string _cost, ECurrencyType _currencyType, Vector3 _funcButtonPos)
    {
        textFuncName.text = _funcName + " (" + _funcHotkey + ")";

        if (_currencyType.Equals(ECurrencyType.NONE))
        {
            imageCostCategory.color = Color.clear;
            textCost.text = null;
        }
        else
        {
            imageCostCategory.color = Color.white;
            imageCostCategory.sprite = arrSpriteCostCategory[(int)_currencyType];
            textCost.text = _cost;
        }
        myRt.position = _funcButtonPos + tooltipOffset;
    }


    [SerializeField]
    private TMP_Text textFuncName = null;
    [SerializeField]
    private TMP_Text textCost = null;
    [SerializeField]
    private Image imageCostCategory = null;
    [SerializeField]
    private Sprite[] arrSpriteCostCategory = null;

    private Vector3 tooltipOffset = Vector3.zero;
    private RectTransform myRt = null;
}
