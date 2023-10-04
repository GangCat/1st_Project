using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHeroRessurection : MonoBehaviour
{
    public void Init()
    {
        textTimer = GetComponentInChildren<TextRessurectionTimer>();
        textTimer.Init();
        SetActive(false);
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void UpdateTimer(float _remainingTime)
    {
        textTimer.UpdateText((int)_remainingTime);
    }

    private TextRessurectionTimer textTimer = null;
}
