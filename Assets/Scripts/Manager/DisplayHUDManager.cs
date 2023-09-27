using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHUDManager : MonoBehaviour
{
    public void Init()
    {
        canvasEnergy = GetComponentInChildren<CanvasDisplayEnergy>();
        canvasCore = GetComponentInChildren<CanvasDisplayCore>();
    }

    public void UpdateEnergy(uint _curEnergy)
    {
        canvasEnergy.UpdateEnergy(_curEnergy);
    }

    public void UpdateCore(uint _curCore)
    {
        canvasCore.UpdateCore(_curCore);
    }

    private CanvasDisplayEnergy canvasEnergy = null;
    private CanvasDisplayCore canvasCore = null;
}
