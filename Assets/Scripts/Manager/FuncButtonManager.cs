using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncButtonManager : MonoBehaviour
{
    public void Init()
    {
        canvasUnitBaseFunc = GetComponentInChildren<CanvasUnitBaseFunc>();
        canvasStructureBaseFunc = GetComponentInChildren<CanvasStructureBaseFunc>();
        canvasBuildFunc = GetComponentInChildren<CanvasBuildFunc>();
        canvasBarrackFunc = GetComponentInChildren<CanvasBarrackFunc>();
        canvasTurretFunc = GetComponentInChildren<CanvasTurretFunc>();

        canvasUnitBaseFunc.Init();
        canvasStructureBaseFunc.Init();
        canvasBuildFunc.Init();
        canvasBarrackFunc.Init();
        canvasTurretFunc.Init();

        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON, new CommandDisplayUnitCancleButton(canvasUnitBaseFunc));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.HIDE_CANCLE_BUTTON, new CommandHideUnitCancleButton(canvasUnitBaseFunc));

        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON, new CommandDisplayStructureCancleButton(canvasStructureBaseFunc));
        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.HIDE_CANCLE_BUTTON, new CommandHideStructureCancleButton(canvasStructureBaseFunc));

        ArrayChangeHotkeyCommand.Add(EChangeHotkeyCommand.CONFIRM_UNIT_FUNC_BUTTON, new CommandConfirmChangeUnitFuncHotkey(canvasUnitBaseFunc));
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
                canvasBuildFunc.DisplayCanvas();
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
                canvasBuildFunc.DisplayBuildWallFunc();
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.DisplayCanvas();
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.DisplayCanvas();
                canvasBarrackFunc.DisplayCanvas();
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
                canvasBuildFunc.HideCanvas();
                break;
            case EObjectType.TURRET:
                canvasStructureBaseFunc.HideCanvas();
                canvasTurretFunc.HideCanvas();
                break;
            case EObjectType.BUNKER:
                canvasStructureBaseFunc.HideCanvas();
                canvasStructureBaseFunc.HideBunkerFunc();
                canvasBuildFunc.HideBuildWallFunc();
                break;
            case EObjectType.WALL:
                canvasStructureBaseFunc.HideCanvas();
                break;
            case EObjectType.BARRACK:
                canvasStructureBaseFunc.HideCanvas();
                canvasBarrackFunc.HideCanvas();
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
    private CanvasBuildFunc canvasBuildFunc = null;
    private CanvasBarrackFunc canvasBarrackFunc = null;
    private CanvasTurretFunc canvasTurretFunc = null;

    private EObjectType curActiveBtnFunc = EObjectType.NONE;
}