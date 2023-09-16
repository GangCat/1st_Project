using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        inputMng = FindAnyObjectByType<InputManager>();
        cameraMng = FindAnyObjectByType<CameraManager>();
        selectMng = FindAnyObjectByType<SelectableObjectManager>();
        uiMng = FindAnyObjectByType<UIManager>();
        buildMng = FindAnyObjectByType<StructureManager>();
        pathMng = FindAnyObjectByType<PF_PathRequestManager>();
    }

    private void Start()
    {
        // ���콺 ���α�
        Cursor.lockState = CursorLockMode.Confined;

        InitCommandList();

        pathMng.Init();
        grid = pathMng.GetComponent<PF_Grid>();
        selectMng.Init(UnitSelect, grid);
        inputMng.Init(
            MoveUnitByPicking,
            MoveUnitByPickingObject,
            ZoomCamera, 
            MoveCameraWithMouse, 
            MoveCameraWithKey, 
            AddSelectedObject, 
            RemoveSelectedObject, 
            SelectFinish,
            MoveCameraWithObject,
            AttackMove,
            PatrolMove);
        cameraMng.Init();
        uiMng.Init();
        buildMng.Init(grid);
    }

    private void InitCommandList()
    {
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.CANCLE, new CommandButtonCancle());
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.MOVE, new CommandButtonMove(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.STOP, new CommandButtonStop(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.HOLD, new CommandButtonHold(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.PATROL, new CommandButtonPatrol(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.ATTACK, new CommandButtonAttack(inputMng));

        ArrayBuildCommand.Add(EMainBaseCommnad.CANCLE, new CommandBuildCancle(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.CONFIRM, new CommandBuildConfirm(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_TURRET, new CommandBuildTurret(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_BUNKER, new CommandBuildBunker(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_WALL, new CommandBuildWall(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_NUCLEAR, new CommandBuildNuclear(buildMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_BARRACK, new CommandBuildBarrack(buildMng, inputMng));

        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT, new CommandRallypoint(inputMng));
    }

    private void UnitSelect(ESelectableObjectType _selectObjectType)
    {
        uiMng.ShowFuncButton(_selectObjectType);
    }

    private void MoveUnitByPicking(Vector3 _pickPos)
    {
        if (selectMng.IsListEmpty) return;

        if (selectMng.IsFriendlyUnit)
        {

            // ���⼭ �� unit�� pathrequest�� ����?
            
            selectMng.MoveUnitByPicking(_pickPos);
        }
    }

    private void MoveUnitByPickingObject(Transform _targetTr)
    {
        if (selectMng.IsListEmpty) return;

        if (selectMng.IsFriendlyUnit)
        {
            selectMng.MoveUnitByPicking(_targetTr);
        }
    }

    private void ZoomCamera(float _zoomRatio)
    {
        cameraMng.ZoomCamera(_zoomRatio);
    }

    private void MoveCameraWithMouse(Vector2 _mousePos)
    {
        cameraMng.MoveCameraWithMouse(_mousePos);
    }

    private void MoveCameraWithKey(Vector2 _arrowKeyInput)
    {
        cameraMng.MoveCameraWithKey(_arrowKeyInput);
    }

    private void MoveCameraWithObject()
    {
        cameraMng.MoveCameraWithObject(selectMng.GetFirstSelectableObjectInList.GetPos);
    }

    private void AddSelectedObject(SelectableObject _object)
    {
        selectMng.AddSelectedObject(_object);
        // ���õ� obj �ƿ����� ǥ��
    }

    private void RemoveSelectedObject(SelectableObject _object)
    {
        selectMng.RemoveSelectedObject(_object);
        // �ش� obj �ƿ����� ����
    }

    private void SelectFinish()
    {
        // ��ȯ�Ǵ� �迭�� ������ �ش� �迭�� ������Ʈ���� �� �ؿ� �ձ� ���� ����
        selectMng.SelectFinish();
    }

    private void AttackMove(Vector3 _targetPos)
    {
        selectMng.MoveUnitByPicking(_targetPos, true);
    }

    private void PatrolMove(Vector3 _wayPointTo)
    {
        selectMng.Patrol(_wayPointTo);
    }

    public void OnClickMoveButton()
    {
        inputMng.OnClickMoveButton();
    }

    private void BuildButtonOnClick(int _buildingType)
    {
        buildMng.ShowBluepirnt((ESelectableObjectType)_buildingType);
    }




    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
    private UIManager uiMng = null;
    private StructureManager buildMng = null;
    private PF_PathRequestManager pathMng = null;

    private PF_Grid grid = null;
}
