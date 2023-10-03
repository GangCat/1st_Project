using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProgressbar : MonoBehaviour
{
    public void Init()
    {
        maxLength = imageBack.GetComponent<RectTransform>().rect.width;
        myRt = GetComponent<RectTransform>();
        myHeight = myRt.rect.height;
    }

    public void UpdateLength(float _ratio)
    {
        myRt.sizeDelta = new Vector2(maxLength * _ratio, myHeight);
    }

    [SerializeField]
    private Image imageBack = null;

    private float maxLength = 0f;
    private float myHeight = 0f; 
    private RectTransform myRt;
}
