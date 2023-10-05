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
        ArrayHUDCommand.Add(EHUDCommand.FINISH_SPAWN_UNIT, new CommandFinishSpawnUnit(canvasSpawnUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.DISPLAY_SPAWN_UNIT_INFO, new CommandDisplaySpawnUnitInfo(canvasSpawnUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.UPDATE_SPAWN_UNIT_PROGRESS, new CommandUpdateSpawnUnitProgress(canvasSpawnUnitInfo));
        ArrayHUDCommand.Add(EHUDCommand.HIDE_ALL_INFO, new CommandHideAllInfo(canvasUnitInfo, canvasSpawnUnitInfo, canvasUpgradeInfo));

        ArrayHUDUpgradeCommand.Add(EHUDUpgradeCommand.DISPLAY, new CommandDisplayUpgradeProgress(canvasUpgradeInfo));
        ArrayHUDUpgradeCommand.Add(EHUDUpgradeCommand.UPDATE_PROGRESS, new CommandUpdateUpgradeProgress(canvasUpgradeInfo));
    }

    public void HideAllInfo()
    {
        canvasUnitInfo.HideDisplay();
        canvasSpawnUnitInfo.HideDisplay();
        canvasUpgradeInfo.HideDisplay();
    }

    public void UpgradeFinish()
    {
        canvasUpgradeInfo.UpgradeFinish();
    }

    public void UpgradeMainbase(EUpgradeETCType _type)
    {
        canvasUpgradeInfo.UpgradeMainbase(_type);
    }

    public void UpgradeStructure()
    {
        canvasUpgradeInfo.UpgradeStructure();
    }

    public void UpgradeUnit(EUnitUpgradeType _type)
    {
        canvasUpgradeInfo.UpgradeUnit(_type);
    }

    public void SpawnUnit(EUnitType _type)
    {
        canvasSpawnUnitInfo.AddSpawnQueue(_type);
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
