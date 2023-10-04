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
    protected Image imageBack = null;

    protected float maxLength = 0f;
    protected float myHeight = 0f;
    protected RectTransform myRt;
}
