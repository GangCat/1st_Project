using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSpawnNuclearInfo : MonoBehaviour
{
    public void Init()
    {
        imageProgressbar.Init();
        SetActive(false);
    }

    public void ShowDisplay()
    {
        SetActive(true);
    }

    public void HideDisplay()
    {
        SetActive(false);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void UpdateSpawnNuclearTime(float _percent)
    {
        imageProgressbar.UpdateLength(_percent);
    }


    [SerializeField]
    private ImageProgressbar imageProgressbar = null;
}
