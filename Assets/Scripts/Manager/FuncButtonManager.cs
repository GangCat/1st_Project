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

        canvasUnitBaseFunc.Init();
        canvasStructureBaseFunc.Init();
        canvasMainBaseFunc.Init();
        canvasBarrackFunc.Init();
        canvasBarrackUpgradeFunc.Init();
        canvasTurretFunc.Init();
        canvasBunkerFunc.Init();

        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON, new CommandDisplayUnitCancleButton(canvasUnitBaseFunc));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.HIDE_CANCLE_BUTTON, new CommandHideUnitCancleButton(canvasUnitBaseFunc));

        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON, new CommandDisplayStructureCancleButton(canvasStructureBaseFunc));
        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.HIDE_CANCLE_BUTTON, new CommandHideStructureCancleButton(canvasStructureBaseFunc));

        ArrayChangeHotkeyCommand.Add(EChangeHotkeyCommand.CONFIRM_UNIT_FUNC_BUTTON, new CommandConfirmChangeUnitFuncHotkey(canvasUnitBaseFunc));
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
            case EObjectType.UNIT_01:
                canvasUnitBaseFunc.DisplayCanvas();
                break;
            case EObjectType.UNIT_02:
                canvasUnitBaseFunc.DisplayCanvas();
                break;
            case EObjectType.UNIT_HERO:
                canvasUnitBaseFunc.DisplayCanvas();
                canvasUnitBaseFunc.DisplayLaunchNuclearButton();
                break;
            case EObjectType.MAIN_BASE:
                canvasMainBaseFunc.DisplayCanvas();
                canvasStructureBaseFunc.DisplayCanvas();
                canvasStructureBaseFunc.DisplayMainbaseFunc();
                break;
            case EObjectType.TURRET:
                canvasStructureBaseFunc.DisplayCanvas();
                canvasTurretFunc.DisplayCanvas();
                break;
            case EObjectType.BUNKER:
                canvasStructureBaseFunc.DisplayCanvas();
                canvasStructureBaseFunc.DisplayBunkerFunc();
                canvasBunkerFunc.DisplayCanvas();
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.DisplayCanvas();
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.DisplayCanvas();
                canvasBarrackFunc.DisplayCanvas();
                canvasBarrackUpgradeFunc.DisplayCanvas();
                break;
            case EObjectType.NUCLEAR:
                canvasStructureBaseFunc.DisplayCanvas();
                canvasStructureBaseFunc.DisplayNuclearStructureFunc();
                break;
            default:
                break;
        }

        curActiveBtnFunc = _selectObjectType;
    }

    private void HideFuncButton()
    {
        canvasStructureBaseFunc.HideCanvas();

        switch (curActiveBtnFunc)
        {
            case EObjectType.UNIT_01:
                canvasUnitBaseFunc.HideCanvas();
                break;
            case EObjectType.UNIT_02:
                canvasUnitBaseFunc.HideCanvas();
                break;
            case EObjectType.UNIT_HERO:
                canvasUnitBaseFunc.HideCanvas();
                canvasUnitBaseFunc.HideLaunchNuclearButton();
                break;
            case EObjectType.MAIN_BASE:
                canvasStructureBaseFunc.HideCanvas();
                canvasStructureBaseFunc.HideMainbaseFunc();
                canvasMainBaseFunc.HideCanvas();
                break;
            case EObjectType.TURRET:
                canvasStructureBaseFunc.HideCanvas();
                canvasTurretFunc.HideCanvas();
                break;
            case EObjectType.BUNKER:
                canvasStructureBaseFunc.HideCanvas();
                canvasStructureBaseFunc.HideBunkerFunc();
                canvasBunkerFunc.HideCanvas();
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.HideCanvas();
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.HideCanvas();
                canvasBarrackFunc.HideCanvas();
                canvasBarrackUpgradeFunc.HideCanvas();
                break;
            case EObjectType.NUCLEAR:
                canvasStructureBaseFunc.HideCanvas();
                canvasStructureBaseFunc.HideNuclearStructureFunc();
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

    private EObjectType curActiveBtnFunc = EObjectType.NONE;
}