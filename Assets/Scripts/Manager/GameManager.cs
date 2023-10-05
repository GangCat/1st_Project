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
        populationMng = FindAnyObjectByType<PopulationManager>();
        heroMng = FindAnyObjectByType<HeroUnitManager>();

        mainBaseTr = FindAnyObjectByType<StructureMainBase>().transform;
    }

    private void Start()
    {
        // 마우스 가두기
        Cursor.lockState = CursorLockMode.Confined;
        // 유니티 에디터에서 실행할 때 창 모드로 실행
        //#if UNITY_EDITOR
        //        Screen.SetResolution(Screen.width, Screen.height, false);
        //#endif

        // 빌드된 게임에서 실행할 때 창 모드로 실행
        //#if !UNITY_EDITOR
        //        Screen.SetResolution(1920, 1080, false);
        //#endif

        //빌드된 게임에서 실행할 때 전체 화면 모드로 실행
#if !UNITY_EDITOR
                Screen.SetResolution(1920, 1080, true);

                // 검은 여백 채우기
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
#endif

        InitCommandList();
        InitManagers();
    }

    private void InitManagers()
    {
        pathMng.Init();
        grid = pathMng.GetComponent<PF_Grid>();
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
        structureMng.Init(grid, FindAnyObjectByType<StructureMainBase>());

        uiMng.Init();
        selectMng.Init(UnitSelect, grid);
        enemyMng.Init(grid, mainBaseTr.position);
        currencyMng.Init();
        populationMng.Init();

        heroMng.Init(FindAnyObjectByType<UnitHero>());
        InitMainBase();
    }

    private void InitCommandList()
    {
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.CANCLE, new CommandButtonCancle());
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.MOVE, new CommandButtonMove(inputMng));
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.STOP, new CommandButtonStop(selectMng));
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.HOLD, new CommandButtonHold(selectMng));
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.PATROL, new CommandButtonPatrol(inputMng));
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.ATTACK, new CommandButtonAttack(inputMng));
        ArrayFuncButtonCommand.Add(EFuncButtonCommand.LAUNCH_NUCLEAR, new CommandButtonLaunchNuclear(inputMng));

        ArrayBuildCommand.Add(EMainBaseCommnad.CANCLE, new CommandBuildCancle(structureMng, inputMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.CONFIRM, new CommandBuildConfirm(structureMng, inputMng, currencyMng));
        ArrayBuildCommand.Add(EMainBaseCommnad.BUILD_STRUCTURE, new CommandBuildStructure(structureMng, inputMng, currencyMng));

        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT, new CommandRallypoint(inputMng));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_UNIT, new CommandSpawnUnit(selectMng, currencyMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_POS, new CommandConfirmRallyPointPos(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_TR, new CommandConfirmRallyPointTr(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_UNIT, new CommandUpgradeUnit(selectMng, currencyMng));

        ArrayBunkerCommand.Add(EBunkerCommand.IN_UNIT, new CommandInUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ONE_UNIT, new CommandOutOneUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ALL_UNIT, new CommandOutAllUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.EXPAND_WALL, new CommandExpandWall(selectMng, structureMng, inputMng));

        ArrayUICommand.Add(EUICommand.UPDATE_INFO_UI, new CommandUpdateInfoUI(selectMng));

        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.WAVE_ENEMY_DEAD, new CommandWaveEnemyDead(enemyMng));
        ArrayEnemyObjectCommand.Add(EEnemyObjectCommand.MAP_ENEMY_DEAD, new CommandMapEnemyDead(enemyMng));

        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DEAD, new CommandFriendlyDead(structureMng, selectMng, populationMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DESTROY, new CommandFriendlyDestroy(structureMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_DMG, new CommandCompleteUpgradeRangedUnitDmg(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_RANGED_UNIT_HP, new CommandCompleteUpgradeRangedUnitHp(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_DMG, new CommandCompleteUpgradeMeleeUnitDmg(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_UPGRADE_MELEE_UNIT_HP, new CommandCompleteUpgradeMeleeUnitHp(selectMng));
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.DEAD_HERO, new CommandFriendlyDeadHero(heroMng, uiMng, selectMng));

        ArrayNuclearCommand.Add(ENuclearCommand.SPAWN_NUCLEAR, new CommandSpawnNuclear(structureMng, selectMng));
        ArrayNuclearCommand.Add(ENuclearCommand.LAUNCH_NUCLEAR, new CommandLaunchNuclear(structureMng));

        ArrayStructureButtonCommand.Add(EStructureButtonCommand.DEMOLISH, new CommandDemolition(structureMng, selectMng));
        ArrayStructureButtonCommand.Add(EStructureButtonCommand.UPGRADE, new CommandUpgrade(structureMng, selectMng, currencyMng));

        ArrayCurrencyCommand.Add(ECurrencyCommand.COLLECT_CORE, new CommandCollectPowerCore(currencyMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_CORE_HUD, new CommandUpdateCoreHUD(uiMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_ENERGY_HUD, new CommandUpdateEnergyDisplay(uiMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY, new CommandUpgradeEnergySupply(currencyMng, selectMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY_COMPLETE, new CommandUpgradeEnergySupplyComplete(currencyMng));

        ArrayPopulationCommand.Add(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, new CommandUpdateCurMaxPopulationHUD(uiMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, new CommandUpdateCurPopulationHUD(uiMng));
        ArrayPopulationCommand.Add(EPopulationCommand.INCREASE_CUR_POPULATION, new CommandIncreaseCurPopulation(populationMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPGRADE_MAX_POPULATION, new CommandUpgradePopulation(populationMng, currencyMng, selectMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPGRADE_POPULATION_COMPLETE, new CommandUpgradePopulationComplete(populationMng));
    }

    private StructureMainBase InitMainBase()
    {
        StructureMainBase mainBase = FindAnyObjectByType<StructureMainBase>();
        mainBase.Init(grid);
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
        if (!selectMng.IsListEmpty && selectMng.IsFriendlyUnit)
            selectMng.MoveUnitByPicking(_pickPos);
    }

    private void MoveUnitByPickingObject(Transform _targetTr)
    {
        if (!selectMng.IsListEmpty && selectMng.IsFriendlyUnit)
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
    private PopulationManager populationMng = null;
    private HeroUnitManager heroMng = null;

    private PF_Grid grid = null;
    private Transform mainBaseTr = null;
}
