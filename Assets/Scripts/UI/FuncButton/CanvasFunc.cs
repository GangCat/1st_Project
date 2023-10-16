using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFunc : MonoBehaviour
{
    public virtual void DisplayCanvas()
    {
        SetActive(true);
    }

    public virtual void HideCanvas()
    {
        SetActive(false);
    }

    protected virtual void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }
}
