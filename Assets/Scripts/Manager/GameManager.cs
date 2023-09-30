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
        enemyMng = FindAnyObjectByType<EnemyManager>();
        currencyMng = FindAnyObjectByType<CurrencyManager>();

        mainBaseTr = FindAnyObjectByType<StructureMainBase>().transform;
    }

    private void Start()
    {
        // 마우스 가두기
        Cursor.lockState = CursorLockMode.Confined;

        InitCommandList();
        InitManagers();
        InitPlayer();

        SpawnMapEnemy(10);
        //Invoke("StartWave", 30f);
    }

    private void InitManagers()
    {
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
        enemyMng.Init(grid);
        structureMng.Init(grid, InitMainBase());
        currencyMng.Init();
    }

    private void InitCommandList()
    {
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.CANCLE, new CommandButtonCancle());
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.MOVE, new CommandButtonMove(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.STOP, new CommandButtonStop(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.HOLD, new CommandButtonHold(selectMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.PATROL, new CommandButtonPatrol(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.ATTACK, new CommandButtonAttack(inputMng));
        ArrayUnitButtonCommand.Add(EUnitButtonCommand.LAUNCH_NUCLEAR, new CommandButtonLaunchNuclear(inputMng));

        ArrayBuildCommand.Add(EMainBaseCommnad.CANCLE, new CommandBuildCancle(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.CONFIRM, new CommandBuildConfirm(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_STRUCTURE, new CommandBuildStructure(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_COMPLETE, new CommandBuildComplete(selectMng));

        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT, new CommandRallypoint(inputMng));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_UNIT, new CommandSpawnUnit(selectMng, currencyMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_POS, new CommandConfirmRallyPointPos(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_TR, new CommandConfirmRallyPointTr(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_UNIT, new CommandUpgradeUnit(selectMng, currencyMng));
        //ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_RANGED_UNIT_DMG, new CommandUpgradeRangedUnitDmg(selectMng));
        //ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_RANGED_UNIT_HP, new CommandUpgradeRangedUnitHp(selectMng));
        //ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_MELEE_UNIT_DMG, new CommandUpgradeMeleeUnitDmg(selectMng));
        //ArrayBarrackCommand.Add(EBarrackCommand.UPGRAGE_MELEE_UNIT_HP, new CommandUpgradeMeleeUnitHp(selectMng));

        ArrayBunkerCommand.Add(EBunkerCommand.IN_UNIT, new CommandInUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ONE_UNIT, new CommandOutOneUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ALL_UNIT, new CommandOutAllUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.EXPAND_WALL, new CommandExpandWall(selectMng, structureMng, inputMng));

        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.WAVE_ENEMY_DEAD, new CommandWaveEnemyDead(enemyMng));
        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.MAP_ENEMY_DEAD, new CommandMapEnemyDead(enemyMng));

        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DEAD, new CommandFriendlyDead(structureMng, selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DESTROY, new CommandFriendlyDestroy(structureMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_DMG, new CommandCompleteUpgradeRangedUnitDmg(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_HP, new CommandCompleteUpgradeRangedUnitHp(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_DMG, new CommandCompleteUpgradeMeleeUnitDmg(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_HP, new CommandCompleteUpgradeMeleeUnitHp(selectMng));

        ArrayNuclearCommand.Add(ENuclearCommand.SPAWN_NUCLEAR, new CommandSpawnNuclear(structureMng, selectMng));
        ArrayNuclearCommand.Add(ENuclearCommand.LAUNCH_NUCLEAR, new CommandLaunchNuclear(structureMng));

        ArrayStructureButtonCommand.Add(EStructureButtonCommand.DEMOLISH, new CommandDemolition(structureMng, selectMng));
        ArrayStructureButtonCommand.Add(EStructureButtonCommand.UPGRADE, new CommandUpgrade(structureMng, selectMng));

        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_CORE_HUD, new CommandUpdateCoreDisplay(uiMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_ENERGY_HUD, new CommandUpdateEnergyDisplay(uiMng));
    }

    private void InitPlayer()
    {
        FindAnyObjectByType<UnitHero>().Init();
    }

    private void StartWave()
    {
        enemyMng.SpawnWaveEnemy(mainBaseTr.position, 45);
    }

    private void SpawnMapEnemy(int _count)
    {
        enemyMng.SpawnMapEnemy(_count);
    }

    private StructureMainBase InitMainBase()
    {
        StructureMainBase mainBase = FindAnyObjectByType<StructureMainBase>();
        mainBase.Init(0);
        return mainBase;
    }
    private void UnitSelect(EObjectType _selectObjectType)
    {
        uiMng.ShowFuncButton(_selectObjectType);
    }

    #region inputMngCallback
    private void MoveUnitByPicking(Vector3 _pickPos)
    {
        if (selectMng.IsFriendlyUnit)
            selectMng.MoveUnitByPicking(_pickPos);
    }

    private void MoveUnitByPickingObject(Transform _targetTr)
    {
        if (selectMng.IsFriendlyUnit)
            selectMng.MoveUnitByPicking(_targetTr);
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
    #endregion




    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
    private UIManager uiMng = null;
    private StructureManager structureMng = null;
    private PF_PathRequestManager pathMng = null;
    private EnemyManager enemyMng = null;
    private CurrencyManager currencyMng = null;

    private PF_Grid grid = null;
    private Transform mainBaseTr = null;
}
