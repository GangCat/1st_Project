using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjectDisplay : MonoBehaviour
{
    public void Init()
    {
        SetActive(false);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }
}
