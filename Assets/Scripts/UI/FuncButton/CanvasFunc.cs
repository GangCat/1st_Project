using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFunc : MonoBehaviour
{
    public virtual void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }
}
