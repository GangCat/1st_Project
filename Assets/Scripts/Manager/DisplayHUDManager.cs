using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHUDManager : MonoBehaviour
{
    public void Init()
    {
        canvasEnergy = GetComponentInChildren<CanvasDisplayEnergy>();
        canvasCore = GetComponentInChildren<CanvasDisplayCore>();
        canvasPopulation = GetComponentInChildren<CanvasDisplayPopulation>();
        canvasMinimap = GetComponentInChildren<CanvasMinimap>();

        canvasMinimap.Init();
    }

    public void AddStructureNodeToMinimap(PF_Node _node)
    {
        canvasMinimap.AddStructureNodeToMinimap(_node);
    }

    public void RemoveStructureNodeFromMinimap(PF_Node _node)
    {
        canvasMinimap.RemoveStructureNodeFromMinimap(_node);
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
    private CanvasMinimap canvasMinimap = null;
}
