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
        canvasSpawnUnitInfo = GetComponentInChildren<CanvasSpawnUnitInfo>();
        canvasUpgradeInfo = GetComponentInChildren<CanvasUpgradeInfo>();

        canvasMinimap.Init();
        canvasWaveInfo.Init();
        canvasUnitInfo.Init();
        canvaHeroRessurection.Init();
        canvasSpawnUnitInfo.Init();
        canvasUpgradeInfo.Init();

        ArrayHUDCommand.Add(EHUDCommand.INIT_WAVE_TIME, new CommandInitWaveTime(canvasWaveInfo));
        ArrayHUDCommand.Add(EHUDCommand.UPDATE_WAVE_TIME, new CommandUpdateWaveTime(canvasWaveInfo));

        ArrayHUDCommand.Add(EHUDCommand.INIT_DISPLAY_GROUP_INFO, new CommandInitDisplayGroupUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.INIT_DISPLAY_SINGLE_INFO, new CommandInitDisplaySingleUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.DISPLAY_GROUP_INFO, new CommandDisplayGroupUnitInfo(canvasUnitInfo, canvasSpawnUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.DISPLAY_SINGLE_INFO, new CommandDisplaySingleUnitInfo(canvasUnitInfo, canvasSpawnUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.HIDE_UNIT_INFO, new CommandHideUnitInfo(canvasUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.HERO_RESURRECTION_UPDATE, new CommandHeroRessurectionUpdate(canvaHeroRessurection));
        ArrayHUDCommand.Add(EHUDCommand.HERO_RESSURECTION_FINISH, new CommandHeroRessurectionFinish(canvaHeroRessurection));
        ArrayHUDCommand.Add(EHUDCommand.HIDE_ALL_INFO, new CommandHideAllInfo(canvasUnitInfo, canvasSpawnUnitInfo, canvasUpgradeInfo));

        ArrayHUDUpgradeCommand.Add(EHUDUpgradeCommand.UPDATE_PROGRESS, new CommandUpdateUpgradeTime(canvasUpgradeInfo));

        ArrayHUDSpawnUnitCommand.Add(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_LIST, new CommandUpdateSpawnUnitList(canvasSpawnUnitInfo));
        ArrayHUDSpawnUnitCommand.Add(EHUDSpawnUnitCommand.UPDATE_SPAWN_UNIT_TIME, new CommandUpdateSpawnUnitTime(canvasSpawnUnitInfo));
        ArrayHUDSpawnUnitCommand.Add(EHUDSpawnUnitCommand.DISPLAY_SPAWN_UNIT_INFO, new CommandDisplaySpawnUnitInfo(canvasSpawnUnitInfo));
    }

    public void DisplayUpgradeInfo(EUpgradeType _type)
    {
        canvasUpgradeInfo.DisplayUpgradeInfo(_type);
    }

    public void HideAllInfo()
    {
        canvasUnitInfo.HideDisplay();
        canvasSpawnUnitInfo.HideDisplay();
        canvasUpgradeInfo.HideDisplay();
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
    private CanvasSpawnUnitInfo canvasSpawnUnitInfo = null;
    private CanvasUpgradeInfo canvasUpgradeInfo = null;
}
