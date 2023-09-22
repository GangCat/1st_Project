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
        structureMng = FindAnyObjectByType<StructureManager>();
        pathMng = FindAnyObjectByType<PF_PathRequestManager>();
        enemyObjMng = FindAnyObjectByType<EnemyManager>();
    }

    private void Start()
    {
        // 마우스 가두기
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
        structureMng.Init(grid);
        enemyObjMng.Init();

        Invoke("StartWave", 1f);

        InitPlayer();
    }

    private void StartWave()
    {
        enemyObjMng.SpawnWaveEnemy(Vector3.zero, 45);
    }

    private void InitCommandList()
    {
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.CANCLE, new CommandButtonCancle());
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.MOVE, new CommandButtonMove(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.STOP, new CommandButtonStop(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.HOLD, new CommandButtonHold(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.PATROL, new CommandButtonPatrol(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.ATTACK, new CommandButtonAttack(inputMng));

        ArrayBuildCommand.Add(EMainBaseCommnad.CANCLE, new CommandBuildCancle(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.CONFIRM, new CommandBuildConfirm(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_STRUCTURE, new CommandBuildStructure(structureMng, inputMng));

        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT, new CommandRallypoint(inputMng));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_UNIT, new CommandSpawnUnit(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_POS, new CommandConfirmRallyPointPos(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_TR, new CommandConfirmRallyPointTr(selectMng));

        ArrayBunkerCommand.Add(EBunkerCommand.IN_UNIT, new CommandInUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ONE_UNIT, new CommandOutOneUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ALL_UNIT, new CommandOutAllUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.EXPAND_WALL, new CommandExpandWall(selectMng, structureMng, inputMng));

        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.WAVE_ENEMY_DEAD, new CommandWaveEnemyDead(enemyObjMng));
        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.MAP_ENEMY_DEAD, new CommandMapEnemyDead(enemyObjMng));

        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DEAD, new CommandFriendlyDead(structureMng, selectMng));
    }

    private void InitPlayer()
    {
        FindAnyObjectByType<UnitHeroIndicator>().Init();
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

            // 여기서 각 unit의 pathrequest를 진행?

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
        cameraMng.MoveCameraWithObject(selectMng.GetFirstSelectedObjectInList.Position);
    }

    private void AddSelectedObject(SelectableObject _object)
    {
        selectMng.AddSelectedObject(_object);
        // 선택된 obj 아웃라인 표시
    }

    private void RemoveSelectedObject(SelectableObject _object)
    {
        selectMng.RemoveSelectedObject(_object);
        // 해당 obj 아웃라인 해제
    }

    private void SelectFinish()
    {
        // 반환되는 배열을 가지고 해당 배열의 오브젝트들의 발 밑에 둥근 원을 생성
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
        structureMng.ShowBluepirnt((ESelectableObjectType)_buildingType);
    }




    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
    private UIManager uiMng = null;
    private StructureManager structureMng = null;
    private PF_PathRequestManager pathMng = null;
    private EnemyManager enemyObjMng = null;

    private PF_Grid grid = null;
}
