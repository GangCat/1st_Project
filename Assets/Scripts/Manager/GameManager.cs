using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IPauseSubject
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (isInGame)
        {
            selectMng = FindFirstObjectByType<SelectableObjectManager>();
            structureMng = FindFirstObjectByType<StructureManager>();
            pathMng = FindFirstObjectByType<PF_PathRequestManager>();
            enemyMng = FindFirstObjectByType<EnemyManager>();
            currencyMng = FindFirstObjectByType<CurrencyManager>();
            populationMng = FindFirstObjectByType<PopulationManager>();
            heroMng = FindFirstObjectByType<HeroUnitManager>();
            fogMng = FindFirstObjectByType<FogManager>();
            debugMng = FindFirstObjectByType<DebugModeManager>();

            mainBaseTr = FindFirstObjectByType<StructureMainBase>().transform;
        }

        inputMng = FindFirstObjectByType<InputManager>();
        cameraMng = FindFirstObjectByType<CameraManager>();
        uiMng = FindFirstObjectByType<UIManager>();
        
    }

    private void Start()
    {
        // 마우스 가두기
        Cursor.lockState = CursorLockMode.Confined;
        // 마우스 모양 바꾸기
        //Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
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
        RegistObserver();
    }

    private void InitManagers()
    {
        cameraMng.Init();
        uiMng.Init();

        if (isInGame)
        {
            inputMng.Init(mainBaseTr.GetComponent<SelectableObject>(), TriggerDebugMode);

            pathMng.Init(worldSizeX, worldSizeY);
            grid = pathMng.GetComponent<PF_Grid>();
            structureMng.Init(grid, FindFirstObjectByType<StructureMainBase>());

            selectMng.Init(UnitSelect, grid);
            enemyMng.Init(grid, mainBaseTr.position);
            currencyMng.Init();
            populationMng.Init();

            fogMng.Init();
            debugMng.Init();
            heroMng.Init(FindFirstObjectByType<UnitHero>());
            InitMainBase();
        }

        if (isMainMenu)
        {
            inputMng.MainInit();
        }


    }

    private void InitCommandList()
    {
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.CANCLE, new CommandUnitCancle(inputMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.MOVE, new CommandButtonMove(inputMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.STOP, new CommandButtonStop(selectMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.HOLD, new CommandButtonHold(selectMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.PATROL, new CommandButtonPatrol(inputMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.ATTACK, new CommandButtonAttack(inputMng));
        ArrayUnitFuncButtonCommand.Add(EUnitFuncButtonCommand.LAUNCH_NUCLEAR, new CommandButtonLaunchNuclear(inputMng));

        ArrayMainbaseCommand.Add(EMainbaseCommnad.CANCLE, new CommandBuildCancle(structureMng, inputMng));
        ArrayMainbaseCommand.Add(EMainbaseCommnad.CONFIRM, new CommandBuildConfirm(structureMng, inputMng, currencyMng));
        ArrayMainbaseCommand.Add(EMainbaseCommnad.BUILD_STRUCTURE, new CommandBuildStructure(structureMng, inputMng, currencyMng));

        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT, new CommandRallypoint(inputMng));
        ArrayBarrackCommand.Add(EBarrackCommand.SPAWN_UNIT, new CommandSpawnUnit(selectMng, currencyMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_POS, new CommandConfirmRallyPointPos(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.RALLYPOINT_CONFIRM_TR, new CommandConfirmRallyPointTr(selectMng));
        ArrayBarrackCommand.Add(EBarrackCommand.UPGRADE_UNIT, new CommandUpgradeUnit(currencyMng));

        ArrayBunkerCommand.Add(EBunkerCommand.IN_UNIT, new CommandInUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ONE_UNIT, new CommandOutOneUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.OUT_ALL_UNIT, new CommandOutAllUnit(selectMng));
        ArrayBunkerCommand.Add(EBunkerCommand.EXPAND_WALL, new CommandExpandWall(structureMng, inputMng));

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
        ArrayFriendlyObjectCommand.Add(EFriendlyObjectCommand.COMPLETE_SPAWN_UNIT, new CommandCompleteSpawnUnit(selectMng));

        ArrayNuclearCommand.Add(ENuclearCommand.SPAWN_NUCLEAR, new CommandSpawnNuclear(structureMng, currencyMng));
        ArrayNuclearCommand.Add(ENuclearCommand.LAUNCH_NUCLEAR, new CommandLaunchNuclear(structureMng));

        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.DEMOLISH, new CommandDemolition(currencyMng));
        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.UPGRADE, new CommandUpgrade(structureMng, currencyMng));
        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.CANCLE_CURRENT_FUNCTION, new CommandStructureCancle(currencyMng, inputMng));
        ArrayStructureFuncButtonCommand.Add(EStructureButtonCommand.DEMOLISH_COMPLETE, new CommandDemolishComplete(structureMng));

        ArrayCurrencyCommand.Add(ECurrencyCommand.COLLECT_CORE, new CommandCollectPowerCore(currencyMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_CORE_HUD, new CommandUpdateCoreHUD(uiMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPDATE_ENERGY_HUD, new CommandUpdateEnergyDisplay(uiMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY, new CommandUpgradeEnergySupply(currencyMng));
        ArrayCurrencyCommand.Add(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY_COMPLETE, new CommandUpgradeEnergySupplyComplete(currencyMng));

        ArrayPopulationCommand.Add(EPopulationCommand.UPDATE_CURRENT_MAX_POPULATION_HUD, new CommandUpdateCurMaxPopulationHUD(uiMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPDATE_CURRENT_POPULATION_HUD, new CommandUpdateCurPopulationHUD(uiMng));
        ArrayPopulationCommand.Add(EPopulationCommand.INCREASE_CUR_POPULATION, new CommandIncreaseCurPopulation(populationMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPGRADE_MAX_POPULATION, new CommandUpgradePopulation(populationMng, currencyMng));
        ArrayPopulationCommand.Add(EPopulationCommand.UPGRADE_POPULATION_COMPLETE, new CommandUpgradePopulationComplete(populationMng));

        ArraySelectCommand.Add(ESelectCommand.TEMP_SELECT, new CommandTempSelect(selectMng));
        ArraySelectCommand.Add(ESelectCommand.TEMP_UNSELECT, new CommandTempUnselect(selectMng));
        ArraySelectCommand.Add(ESelectCommand.SELECT_FINISH, new CommandSelectFinish(selectMng));
        ArraySelectCommand.Add(ESelectCommand.SELECT_START, new CommandSelectStart(selectMng));
        ArraySelectCommand.Add(ESelectCommand.REMOVE_FROM_LIST, new CommandRemoveFromList(selectMng));
        ArraySelectCommand.Add(ESelectCommand.SET_LIST_TO_CROWD, new CommandSetListToCrowd(selectMng));
        ArraySelectCommand.Add(ESelectCommand.LOAD_CROWD_WITH_IDX, new CommandLoadCrowdWithIdx(selectMng));

        ArrayUnitActionCommand.Add(EUnitActionCommand.MOVE_WITH_POS, new CommandUnitMoveWithPos(selectMng));
        ArrayUnitActionCommand.Add(EUnitActionCommand.MOVE_ATTACK, new CommandUnitMoveAttack(selectMng));
        ArrayUnitActionCommand.Add(EUnitActionCommand.FOLLOW_OBJECT, new CommandUnitFollowObject(selectMng));
        ArrayUnitActionCommand.Add(EUnitActionCommand.PATROL, new CommandUnitPatrol(selectMng));

        ArrayPauseCommand.Add(EPauseCommand.REGIST, new CommandRegistPauseObserver(this));
        ArrayPauseCommand.Add(EPauseCommand.REMOVE, new CommandRemovePauseObserver(this));
        ArrayPauseCommand.Add(EPauseCommand.TOGGLE_PAUSE, new CommandPauseToggle(this, inputMng, structureMng));

        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.BUILD_STRUCTURE, new CommandRefundBuildStructure(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.UPGRADE_STRUCTURE, new CommandRefundUpgradeStructure(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.UPGRADE_UNIT, new CommandRefundUpgradeUnit(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.SPAWN_UNIT, new CommandRefundSpawnUnit(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.UPGRADE_ENERGY, new CommandRefundUpgradeEnergySupply(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.UPGRADE_POPULATION, new CommandRefundUpgradePopulation(currencyMng));
        ArrayRefundCurrencyCommand.Add(ERefuncCurrencyCommand.SPAWN_NUCLEAR, new CommandRefundSpawnNuclear(currencyMng));

        ArrayDebugModeCommand.Add(EDebugModeCommand.MOVE_STATE_INDICATOR, new CommandMoveCurStateIndicator(debugMng));

        ArrayChangeHotkeyCommand.Add(EChangeHotkeyCommand.SELECT_UNIT_FUNC_BUTTON, new CommandChangeUnitFuncHotkey(inputMng));
    }

    private void TriggerDebugMode()
    {
        isDebugMode = !isDebugMode;
        debugMng.SetActive(isDebugMode);
        fogMng.IsDebugMode(isDebugMode);
        heroMng.DebugMode(isDebugMode);
    }

    private void RegistObserver()
    {
        ImageMinimap minimap = FindFirstObjectByType<ImageMinimap>();
        minimap.Init(worldSizeX, worldSizeY);
        minimap.RegisterPauseObserver(inputMng.GetComponent<IMinimapObserver>());
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

    public void RegisterPauseObserver(IPauseObserver _observer)
    {
        pauseObserverList.Add(_observer);
    }

    public void RemovePauseObserver(IPauseObserver _observer)
    {
        pauseObserverList.Remove(_observer);
    }

    public void TogglePause()
    {
        isPause = !isPause;
        for (int i = 0; i < pauseObserverList.Count; ++i)
            pauseObserverList[i].CheckPause(isPause);
    }

    [SerializeField]
    private float worldSizeX = 100f; // 미니맵에 표시할 월드의 가로길이
    [SerializeField]
    private float worldSizeY = 100f; // 미니맵에 표시할 월드의 세로길이
    [SerializeField]
    private Texture2D customCursor = null;

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
    private FogManager fogMng = null;
    private DebugModeManager debugMng = null;

    private PF_Grid grid = null;
    private Transform mainBaseTr = null;

    private List<IPauseObserver> pauseObserverList = new List<IPauseObserver>();
    private bool isPause = false;
    private bool isDebugMode = false;

    [SerializeField]
    private bool isMainMenu = false;
    [SerializeField]
    private bool isInGame = false;
}
