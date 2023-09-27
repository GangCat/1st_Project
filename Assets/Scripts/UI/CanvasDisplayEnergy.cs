using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CanvasDisplayEnergy : MonoBehaviour
{
    public void UpdateEnergy(uint _curEnergy)
    {
        textEnergy.text = string.Format("{0:N0}", _curEnergy);
    }

    [SerializeField]
    private TMP_Text textEnergy = null;
}
