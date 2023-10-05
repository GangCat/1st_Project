using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncButtonManager : MonoBehaviour
{
    public void Init()
    {
        canvasUnitBaseFunc = GetComponentInChildren<CanvasUnitBaseFunc>();
        canvasStructureBaseFunc = GetComponentInChildren<CanvasStructureBaseFunc>();
        canvasMainBaseFunc = GetComponentInChildren<CanvasMainBaseFunc>();
        canvasBarrackFunc = GetComponentInChildren<CanvasBarrackFunc>();
        canvasBarrackUpgradeFunc = GetComponentInChildren<CanvasBarrackUpgradeFunc>();
        canvasTurretFunc = GetComponentInChildren<CanvasTurretFunc>();
        canvasBunkerFunc = GetComponentInChildren<CanvasBunkerFunc>();
        canvasSpawnNuclearFunc = GetComponentInChildren<CanvasSpawnNuclearFunc>();
        canvasHeroFunc = GetComponentInChildren<CanvasHeroFunc>();
        canvasCancleFunc = GetComponentInChildren<CanvasCancleFunc>();

        canvasUnitBaseFunc.Init();
        canvasStructureBaseFunc.Init();
        canvasMainBaseFunc.Init();
        canvasBarrackFunc.Init();
        canvasBarrackUpgradeFunc.Init();
        canvasTurretFunc.Init();
        canvasBunkerFunc.Init();
        canvasSpawnNuclearFunc.Init();
        canvasHeroFunc.Init();
        canvasCancleFunc.Init();
    }

    public void SetBarrackButtonUninteractable()
    {
        canvasBarrackFunc.SetAllButtonUninteractable();
    }

    public void SetBarrackButtonInteractable()
    {
        canvasBarrackFunc.SetAllButtonInteractable();
    }

    public void ShowFuncButton(EObjectType _selectObjectType)
    {
        HideFuncButton();

        switch (_selectObjectType)
        {
            case EObjectType.UNIT:
                canvasUnitBaseFunc.SetActive(true);
                break;
            case EObjectType.UNIT_HERO:
                canvasUnitBaseFunc.SetActive(true);
                canvasHeroFunc.SetActive(true);
                break;
            case EObjectType.MAIN_BASE:
                canvasMainBaseFunc.SetActive(true);
                canvasStructureBaseFunc.SetActive(true);
                break;
            case EObjectType.TURRET:
                canvasStructureBaseFunc.SetActive(true);
                canvasTurretFunc.SetActive(true);
                break;
            case EObjectType.BUNKER:
                canvasStructureBaseFunc.SetActive(true);
                canvasBunkerFunc.SetActive(true);
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.SetActive(true);
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.SetActive(true);
                canvasBarrackFunc.SetActive(true);
                canvasBarrackUpgradeFunc.SetActive(true);
                break;
            case EObjectType.NUCLEAR:
                canvasStructureBaseFunc.SetActive(true);
                canvasSpawnNuclearFunc.SetActive(true);
                break;
            case EObjectType.UNDER_CONSTRUCT:
            case EObjectType.PROCESSING_UPGRADE_STRUCTURE:
                canvasCancleFunc.SetActive(true);
                break;
            default:
                break;
        }

        curActiveBtnFunc = _selectObjectType;
    }

    private void HideFuncButton()
    {
        canvasCancleFunc.SetActive(false);

        switch (curActiveBtnFunc)
        {
            case EObjectType.UNIT:
                canvasUnitBaseFunc.SetActive(false);
                break;
            case EObjectType.UNIT_HERO:
                canvasUnitBaseFunc.SetActive(false);
                canvasHeroFunc.SetActive(false);
                break;
            case EObjectType.MAIN_BASE:
                canvasStructureBaseFunc.SetActive(false);
                canvasMainBaseFunc.SetActive(false);
                break;
            case EObjectType.TURRET:
                canvasStructureBaseFunc.SetActive(false);
                canvasTurretFunc.SetActive(false);
                break;
            case EObjectType.BUNKER:
                canvasStructureBaseFunc.SetActive(false);
                canvasBunkerFunc.SetActive(false);
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.SetActive(false);
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.SetActive(false);
                canvasBarrackFunc.SetActive(false);
                canvasBarrackUpgradeFunc.SetActive(false);
                break;
            case EObjectType.UNDER_CONSTRUCT:
            case EObjectType.PROCESSING_UPGRADE_STRUCTURE:
                canvasCancleFunc.SetActive(false);
                break;
            case EObjectType.NUCLEAR:
                canvasStructureBaseFunc.SetActive(false);
                canvasSpawnNuclearFunc.SetActive(false);
                break;
            default:
                break;
        }
    }

    private CanvasUnitBaseFunc canvasUnitBaseFunc = null;
    private CanvasStructureBaseFunc canvasStructureBaseFunc = null;
    private CanvasMainBaseFunc canvasMainBaseFunc = null;
    private CanvasBarrackFunc canvasBarrackFunc = null;
    private CanvasBarrackUpgradeFunc canvasBarrackUpgradeFunc = null;
    private CanvasTurretFunc canvasTurretFunc = null;
    private CanvasBunkerFunc canvasBunkerFunc = null;
    private CanvasSpawnNuclearFunc canvasSpawnNuclearFunc = null;
    private CanvasHeroFunc canvasHeroFunc = null;
    private CanvasCancleFunc canvasCancleFunc = null;

    private EObjectType curActiveBtnFunc = EObjectType.NONE;
}