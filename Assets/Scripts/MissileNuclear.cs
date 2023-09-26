using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileNuclear : MonoBehaviour
{
    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void Launch(Vector3 _destPos)
    {
        Debug.Log(_destPos + "Launch");
        SetActive(false);
    }
}
