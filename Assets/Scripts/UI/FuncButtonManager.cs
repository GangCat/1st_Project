using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncButtonManager : MonoBehaviour
{
    public void Init(
        VoidVoidDelegate _moveBtnCallback,
        VoidVoidDelegate _cancleBtnCallback)
    {
        canvasUnitBaseFunc = GetComponentInChildren<CanvasUnitBaseFunc>();
        canvasStructureBaseFunc = GetComponentInChildren<CanvasStructureBaseFunc>();
        canvasMainStructureFunc = GetComponentInChildren<CanvasMainBaseFunc>();
        canvasSpawnUnitFunc = GetComponentInChildren<CanvasSpawnUnitFunc>();
        canvasTurretFunc = GetComponentInChildren<CanvasTurretFunc>();
        canvasBunkerFunc = GetComponentInChildren<CanvasBunkerFunc>();
        canvasSpawnNuclearFunc = GetComponentInChildren<CanvasSpawnNuclearFunc>();
        canvasHeroFunc = GetComponentInChildren<CanvasHeroFunc>();
        cancleFunc = GetComponentInChildren<CanvasCancleFunc>();

        canvasUnitBaseFunc.Init(_moveBtnCallback + ActiveCancleBtn);
        canvasStructureBaseFunc.Init();
        canvasMainStructureFunc.Init();
        canvasSpawnUnitFunc.Init();
        canvasTurretFunc.Init();
        canvasBunkerFunc.Init();
        canvasSpawnNuclearFunc.Init();
        canvasHeroFunc.Init();
        cancleFunc.Init(_cancleBtnCallback);
    }

    public void ShowFuncButton(ESelectableObjectType _selectObjectType)
    {
        HideFuncButton();

        switch (_selectObjectType)
        {
            case ESelectableObjectType.UNIT:
                canvasUnitBaseFunc.SetActive(true);
                break;
            case ESelectableObjectType.HERO:
                canvasUnitBaseFunc.SetActive(true);
                canvasHeroFunc.SetActive(true);
                break;
            case ESelectableObjectType.MAIN_BASE:
                canvasStructureBaseFunc.SetActive(true);
                canvasMainStructureFunc.SetActive(true);
                break;
            case ESelectableObjectType.TURRET:
                canvasStructureBaseFunc.SetActive(true);
                canvasTurretFunc.SetActive(true);
                break;
            case ESelectableObjectType.BUNKER:
                canvasStructureBaseFunc.SetActive(true);
                canvasBunkerFunc.SetActive(true);
                break;
            case ESelectableObjectType.WALL:
                canvasStructureBaseFunc.SetActive(true);
                break;
            default:
                break;
        }

        curActiveBtnFunc = _selectObjectType;
    }

    private void ActiveCancleBtn()
    {
        cancleFunc.SetActive(true);
    }

    private void DeActiveCancleBtn()
    {
        cancleFunc.SetActive(false);
    }

    private void HideFuncButton()
    {
        switch (curActiveBtnFunc)
        {
            case ESelectableObjectType.UNIT:
                canvasUnitBaseFunc.SetActive(false);
                break;
            case ESelectableObjectType.HERO:
                canvasUnitBaseFunc.SetActive(false);
                canvasHeroFunc.SetActive(false);
                break;
            case ESelectableObjectType.MAIN_BASE:
                canvasStructureBaseFunc.SetActive(false);
                canvasMainStructureFunc.SetActive(false);
                break;
            case ESelectableObjectType.TURRET:
                canvasStructureBaseFunc.SetActive(false);
                canvasTurretFunc.SetActive(false);
                break;
            case ESelectableObjectType.BUNKER:
                canvasStructureBaseFunc.SetActive(false);
                canvasBunkerFunc.SetActive(false);
                break;
            case ESelectableObjectType.WALL:
                canvasStructureBaseFunc.SetActive(false);
                break;
            default:
                break;
        }
    }

    private CanvasUnitBaseFunc canvasUnitBaseFunc = null;
    private CanvasStructureBaseFunc canvasStructureBaseFunc = null;
    private CanvasMainBaseFunc canvasMainStructureFunc = null;
    private CanvasSpawnUnitFunc canvasSpawnUnitFunc = null;
    private CanvasTurretFunc canvasTurretFunc = null;
    private CanvasBunkerFunc canvasBunkerFunc = null;
    private CanvasSpawnNuclearFunc canvasSpawnNuclearFunc = null;
    private CanvasHeroFunc canvasHeroFunc = null;
    private CanvasCancleFunc cancleFunc = null;

    private ESelectableObjectType curActiveBtnFunc = ESelectableObjectType.None;
}
