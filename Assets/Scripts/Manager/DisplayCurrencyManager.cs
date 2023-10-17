using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCurrencyManager : MonoBehaviour
{
    public void Init()
    {
        canvasEnergy = GetComponentInChildren<CanvasDisplayEnergy>();
        canvasCore = GetComponentInChildren<CanvasDisplayCore>();
        canvasPopulation = GetComponentInChildren<CanvasDisplayPopulation>();
    }

    public void UpdateEnergy(uint _curEnergy)
    {
        canvasEnergy.UpdateEnergy(_curEnergy);
    }

    public void UpdateCore(uint _curCore)
    {
        canvasCore.UpdateCore(_curCore);
    }

    public void UpdateCurPopulation(uint _curPopulation)
    {
        canvasPopulation.UpdateCurPopulation(_curPopulation);
    }

    public void UpdateCurMaxPopulation(uint _curMaxPopulation)
    {
        canvasPopulation.UpdateCurMaxPopulation(_curMaxPopulation);
    }



    private CanvasDisplayEnergy canvasEnergy = null;
    private CanvasDisplayCore canvasCore = null;
    private CanvasDisplayPopulation canvasPopulation = null;
}
