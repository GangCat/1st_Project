using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IMinimapObserver
{
    
    
    public void Init()
    {
        selectArea = GetComponentInChildren<SelectArea>();
        selectArea.Init();
    }
    public bool IsBuildOperation
    {
        get => isBuildOperation;
        set
        {
            ClearCurFunc();
            isBuildOperation = value;
        }
    }

    public Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnClickMoveButton()
    {
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isMoveClick = true;
    }

    public void OnClickAttackButton()
    {
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isAttackClick = true;
    }

    public void OnClickPatrolButton()
    {
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isPatrolClick = true;
    }

    public void OnClickRallyPointButton()
    {
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isRallyPointClick = true;
    }

    public void OnClickLaunchNuclearButton()
    {
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isLaunchNuclearClick = true;
    }

    private void ClearCurFunc()
    {
        isMoveClick = false;
        isAttackClick = false;
        isPatrolClick = false;
        isRallyPointClick = false;
        isBuildOperation = false;
        isLaunchNuclearClick = false;
        // 등등 기능과 관련된 bool값 모두 초기화
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        RaycastHit hit;
        if (pickPosDisplayGo != null && Functions.Picking(out hit))
            pickPosDisplayGo.transform.position = hit.point;

        if (Input.anyKey)
            CheckIsHotkey();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (IsBuildOperation)
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.CONFIRM);
                return;
            }
            else if (isAttackClick)
                AttackMoveWithMouseClick();
            else if (isMoveClick)
                MoveWithMouseClick();
            else if (isPatrolClick)
                PatrolWithMouseClick();
            else if (isRallyPointClick)
                SetRallyPoint();
            else if (isLaunchNuclearClick)
                LaunchNuclear();
            else
                DragOperateWithMouseClick();

            if (pickPosDisplayGo != null)
                Destroy(pickPosDisplayGo);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (IsBuildOperation)
            {
                ArrayBuildCommand.Use(EMainBaseCommnad.CANCLE);
                return;
            }

            if (isAttackClick || isMoveClick || isPatrolClick || isRallyPointClick)
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
            }
            else
                MoveWithMouseClick();
        }
    }

    private void CheckIsHotkey()
    {
        if (SelectableObjectManager.IsListEmpty) return;

        EObjectType objType = SelectableObjectManager.GetFirstSelectedObjectInList.GetObjectType();
        switch (objType)
        {
            case EObjectType.UNIT:
                if (UnitDefaultHotkeyAction())
                    break;
                break;
            case EObjectType.UNIT_HERO:
                if (UnitDefaultHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.LAUNCH_NUCLEAR]))
                    ArrayFuncButtonCommand.Use(EFuncButtonCommand.LAUNCH_NUCLEAR);
                break;
            case EObjectType.MAIN_BASE:
                if (StructureDefaultHotkeyAction())
                    break;
                if (MainbaseBuildHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.UPGRADE_ENERGY_SUPPLY]))
                    ArrayCurrencyCommand.Use(ECurrencyCommand.UPGRADE_ENERGY_SUPPLY);
                else if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.UPGRADE_POPULATION_MAX]))
                    ArrayPopulationCommand.Use(EPopulationCommand.UPGRADE_MAX_POPULATION);

                break;
            case EObjectType.TURRET:
                StructureDefaultHotkeyAction();
                break;
            case EObjectType.BUNKER:
                if (StructureDefaultHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.OUT_ONE_UNIT]))
                    ArrayBunkerCommand.Use(EBunkerCommand.OUT_ONE_UNIT);
                else if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.OUT_ALL_UNIT]))
                    ArrayBunkerCommand.Use(EBunkerCommand.OUT_ALL_UNIT);
                else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.NUCLEAR]))
                    ArrayBunkerCommand.Use(EBunkerCommand.EXPAND_WALL);
                break;
            case EObjectType.WALL:
                StructureDefaultHotkeyAction();
                break;
            case EObjectType.BARRACK:
                if (StructureDefaultHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.SPAWN_MELEE]))
                    ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, EUnitType.MELEE);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.SPAWN_RANGED]))
                    ArrayBarrackCommand.Use(EBarrackCommand.SPAWN_UNIT, EUnitType.RANGED);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.SET_RALLYPOINT]))
                    ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.UPGRADE_RANGED_DMG]))
                    ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.RANGED_UNIT_DMG);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.UPGRADE_RANGED_HP]))
                    ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.RANGED_UNIT_HP);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.UPGRADE_MELEE_DMG]))
                    ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.MELEE_UNIT_DMG);
                else if (Input.GetKeyDown(arrBarrackFuncHotkey[(int)EBarrackFuncHotkey.UPGRADE_MELEE_HP]))
                    ArrayBarrackCommand.Use(EBarrackCommand.UPGRADE_UNIT, EUnitUpgradeType.MELEE_UNIT_HP);
                break;
            case EObjectType.NUCLEAR:
                if (StructureDefaultHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.SPAWN_NUCLEAR]))
                    ArrayNuclearCommand.Use(ENuclearCommand.SPAWN_NUCLEAR);
                break;
            case EObjectType.UNDER_CONSTRUCT:
            case EObjectType.PROCESSING_UPGRADE_STRUCTURE:
                if (Input.GetKeyDown(cancleKey))
                    Debug.Log("Cancle");
                break;
            default:
                break;
        }
    }

    private bool StructureDefaultHotkeyAction()
    {
        if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.DEMOLISH]))
            ArrayStructureButtonCommand.Use(EStructureButtonCommand.DEMOLISH);
        else if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.UPGRADE]))
            ArrayStructureButtonCommand.Use(EStructureButtonCommand.UPGRADE);
        else
            return false;
        return true;
    }

    private bool UnitDefaultHotkeyAction()
    {
        if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.MOVE]))
            ArrayFuncButtonCommand.Use(EFuncButtonCommand.MOVE);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.STOP]))
            ArrayFuncButtonCommand.Use(EFuncButtonCommand.STOP);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.HOLD]))
            ArrayFuncButtonCommand.Use(EFuncButtonCommand.HOLD);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.PATROL]))
            ArrayFuncButtonCommand.Use(EFuncButtonCommand.PATROL);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.ATTACK]))
            ArrayFuncButtonCommand.Use(EFuncButtonCommand.ATTACK);
        else
            return false;

        return true;
    }

    private bool MainbaseBuildHotkeyAction()
    {
        if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.TURRET]))
            ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.TURRET);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.BUNKER]))
            ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.BUNKER);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.BARRACK]))
            ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.BARRACK);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.NUCLEAR]))
            ArrayBuildCommand.Use(EMainBaseCommnad.BUILD_STRUCTURE, EObjectType.NUCLEAR);
        else
            return false;
        return true;
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
    }

    private void SetRallyPoint()
    {
        Vector3 pickPos = Vector3.zero;
        RaycastHit hit;

        if (Functions.Picking(selectableLayer, out hit))
            ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT_CONFIRM_TR, hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
            ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT_CONFIRM_POS, pickPos);

        GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
        StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);

        ClearCurFunc();
    }

    private void LaunchNuclear()
    {
        Vector3 pickPos = Vector3.zero;

        if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
            ArrayNuclearCommand.Use(ENuclearCommand.LAUNCH_NUCLEAR, pickPos);

        ClearCurFunc();
    }

    private void MoveWithMouseClick()
    {
        if (elapsedTime < 0.2f)
            return;
        else
            elapsedTime = 0f;


        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 pickPos = Vector3.zero;
        RaycastHit hit;
        if (Functions.Picking(selectableLayer, out hit))
            ArrayUnitActionCommand.Use(EUnitActionCommand.FOLLOW_OBJECT, hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            ArrayUnitActionCommand.Use(EUnitActionCommand.MOVE_WITH_POS, pickPos);
        }

        ClearCurFunc();
    }

    private void AttackMoveWithMouseClick()
    {
        if (elapsedTime < 0.2f)
            return;
        else
            elapsedTime = 0f;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 pickPos = Vector3.zero;
        RaycastHit hit;
        if (Functions.Picking(selectableLayer, out hit))
            ArrayUnitActionCommand.Use(EUnitActionCommand.FOLLOW_OBJECT, hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            ArrayUnitActionCommand.Use(EUnitActionCommand.MOVE_ATTACK, pickPos);
        }

        ClearCurFunc();
    }

    private void PatrolWithMouseClick()
    {
        if (elapsedTime < 0.2f)
            return;
        else
            elapsedTime = 0f;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 pickPos = Vector3.zero;
        RaycastHit hit;
        if (Functions.Picking(selectableLayer, out hit))
            ArrayUnitActionCommand.Use(EUnitActionCommand.FOLLOW_OBJECT, hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            ArrayUnitActionCommand.Use(EUnitActionCommand.PATROL, pickPos);
        }

        ClearCurFunc();
    }

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    private void DragOperateWithMouseClick()
    {
        RaycastHit hit;
        ArraySelectCommand.Use(ESelectCommand.SELECT_START);

        if (Functions.Picking(selectableLayer, out hit))
        {
            SelectableObject sObj = hit.transform.GetComponent<SelectableObject>();
            if (sObj != null)
                ArraySelectCommand.Use(ESelectCommand.TEMP_SELECT, sObj);
        }

        Functions.Picking("StageFloor", floorLayer, ref dragStartPos);
        selectArea.SetPos(dragStartPos);
        selectArea.SetLocalScale(Vector3.zero);
        selectArea.SetActive(true);

        StartCoroutine("DragCoroutine");
    }

    private IEnumerator DragCoroutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
                break;

            Functions.Picking("StageFloor", floorLayer, ref dragEndPos);
            selectArea.SetLocalScale(Quaternion.Euler(0f, -45f, 0f) * (dragEndPos - dragStartPos));

            yield return null;
        }

        ArraySelectCommand.Use(ESelectCommand.SELECT_FINISH);
        selectArea.SetActive(false);
    }

    private void ZoomCamera()
    {
        ArrayCameraMoveCommand.Use(ECameraCommand.ZOOM, Input.GetAxisRaw("Mouse ScrollWheel"));
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if(!SelectableObjectManager.IsListEmpty)
                ArrayCameraMoveCommand.Use(ECameraCommand.MOVE_WITH_OBJECT);
        }
        else if (Input.GetAxisRaw("Horizontal Arrow").Equals(0) && Input.GetAxisRaw("Vertical Arrow").Equals(0))
            ArrayCameraMoveCommand.Use(ECameraCommand.MOVE_WITH_MOUSE, Input.mousePosition);
        else
            ArrayCameraMoveCommand.Use(
                ECameraCommand.MOVE_WITH_KEY,
                new Vector2(Input.GetAxisRaw("Horizontal Arrow"), Input.GetAxisRaw("Vertical Arrow"))
                );
    }

    public void GetUnitTargetPos(Vector3 _pos)
    {
        ArrayUnitActionCommand.Use(EUnitActionCommand.MOVE_WITH_POS, _pos);
    }

    public void GetCameraTargetPos(Vector3 _pos)
    {
        ArrayCameraMoveCommand.Use(ECameraCommand.WARP_WITH_POS, _pos);
    }

    [SerializeField]
    private GameObject pickPosPrefab = null;
    [SerializeField]
    private float pickPosDisplayHideDelay = 0.3f;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private LayerMask selectableLayer;

    [Header("-Hotkeys")]
    [SerializeField]
    private KeyCode[] arrUnitFuncHotkey = null;
    [SerializeField]
    private KeyCode[] arrBuildFuncHotkey = null;
    [SerializeField]
    private KeyCode[] arrBarrackFuncHotkey = null;
    [SerializeField]
    private KeyCode[] arrStructureFuncHotkey = null;
    [SerializeField]
    private KeyCode cancleKey = KeyCode.Escape;

    private float elapsedTime = 0f;

    private bool isMoveClick = false;
    private bool isAttackClick = false;
    private bool isPatrolClick = false;
    private bool isBuildOperation = false;
    private bool isRallyPointClick = false;
    private bool isLaunchNuclearClick = false;

    private GameObject pickPosDisplayGo = null;

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragEndPos = Vector3.zero;

    private SelectArea selectArea = null;

    private enum EUnitFuncHotkey { NONE = -1, MOVE, STOP, HOLD, PATROL, ATTACK, LAUNCH_NUCLEAR, LENGTH }
    private enum EBuildFuncHotkey { NONE = -1, TURRET, BUNKER, BARRACK, NUCLEAR, WALL, LENGTH }
    private enum EBarrackFuncHotkey { NONE = -1, SPAWN_MELEE, SPAWN_RANGED, SET_RALLYPOINT, UPGRADE_RANGED_DMG, UPGRADE_RANGED_HP, UPGRADE_MELEE_DMG, UPGRADE_MELEE_HP, LENGTH }
    private enum EStructureFuncHotkey { NONE = -1, UPGRADE, DEMOLISH, SPAWN_NUCLEAR, OUT_ONE_UNIT, OUT_ALL_UNIT, UPGRADE_ENERGY_SUPPLY, UPGRADE_POPULATION_MAX, LENGTH }
}