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
        canvasWaveInfo = GetComponentInChildren<CanvasWaveInfo>();
        canvasUnitInfo = GetComponentInChildren<CanvasUnitInfo>();
        canvaHeroRessurection = GetComponentInChildren<CanvasHeroRessurection>();

        canvasMinimap.Init();
        canvasWaveInfo.Init();
        canvasUnitInfo.Init();
        canvaHeroRessurection.Init();

        ArrayHUDCommand.Add(EHUDCommand.INIT_WAVE_TIME, new CommandInitWaveTime(canvasWaveInfo));
        ArrayHUDCommand.Add(EHUDCommand.UPDATE_WAVE_TIME, new CommandUpdateWaveTime(canvasWaveInfo));

        ArrayHUDCommand.Add(EHUDCommand.INIT_DISPLAY_GROUP_INFO, new CommandInitDisplayGroupUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.INIT_DISPLAY_SINGLE_INFO, new CommandInitDisplaySingleUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.DISPLAY_GROUP_INFO, new CommandDisplayGroupUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.DISPLAY_SINGLE_INFO, new CommandDisplaySingleUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.HIDE_UNIT_INFO, new CommandHideUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.HERO_RESURRECTION_UPDATE, new CommandHeroRessurectionUpdate(canvaHeroRessurection));
        ArrayHUDCommand.Add(EHUDCommand.HERO_RESSURECTION_FINISH, new CommandHeroRessurectionFinish(canvaHeroRessurection));
    }

    public void HeroDead()
    {
        canvaHeroRessurection.SetActive(true);
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
    private CanvasWaveInfo canvasWaveInfo = null;
    private CanvasUnitInfo canvasUnitInfo = null;
    private CanvasHeroRessurection canvaHeroRessurection = null;
}
