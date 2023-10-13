using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IMinimapObserver, IPauseObserver
{
    public void Init()
    {
        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
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
        AudioManager.instance.PlayAudio_UI(objectType); // CLICK Audio
        
        if (isMoveClick) return;
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isMoveClick = true;
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON);
    }

    public void OnClickAttackButton()
    {
        AudioManager.instance.PlayAudio_UI(objectType); // CLICK Audio
        
        if (isAttackClick) return;
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isAttackClick = true;
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON);
    }

    public void OnClickPatrolButton()
    {
        AudioManager.instance.PlayAudio_UI(objectType); // CLICK Audio
        
        if (isPatrolClick) return;
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isPatrolClick = true;
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON);
    }

    public void OnClickRallyPointButton()
    {
        AudioManager.instance.PlayAudio_UI(objectType); // CLICK Audio
        
        if (isRallyPointClick) return;
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isRallyPointClick = true;
        ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DISPLAY_CANCLE_BUTTON);
    }

    public void OnClickLaunchNuclearButton()
    {
        AudioManager.instance.PlayAudio_UI(objectType); // CLICK Audio
        
        if (isLaunchNuclearClick) return;
        ClearCurFunc();
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isLaunchNuclearClick = true;
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.DISPLAY_CANCLE_BUTTON);
    }

    public void CancleFunc()
    {
        isMoveClick = false;
        isAttackClick = false;
        isPatrolClick = false;
        isBuildOperation = false;
        isLaunchNuclearClick = false;
        Destroy(pickPosDisplayGo);
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.HIDE_CANCLE_BUTTON);
    }

    public void CancleRallypoint()
    {
        if (isRallyPointClick)
        {
            isRallyPointClick = false;
            Destroy(pickPosDisplayGo);
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.HIDE_CANCLE_BUTTON);
        }
    }

    private void ClearCurFunc()
    {
        isMoveClick = false;
        isAttackClick = false;
        isPatrolClick = false;
        isRallyPointClick = false;
        isBuildOperation = false;
        isLaunchNuclearClick = false;
        Destroy(pickPosDisplayGo);
        // 등등 기능과 관련된 bool값 모두 초기화
        ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.HIDE_CANCLE_BUTTON);
        ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.HIDE_CANCLE_BUTTON);
    }

    private void Update()
    {
        if (isPause) return;

        elapsedTime += Time.deltaTime;
        if (isCheckDoubleClick)
        {
            if (leftClickElapsedTime > 0.5f)
            {
                isCheckDoubleClick = false;
                leftClickElapsedTime = 0f;
            }
            else
                leftClickElapsedTime += Time.deltaTime;
        }

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
                ArrayMainbaseCommand.Use(EMainbaseCommnad.CONFIRM);
                return;
            }
            else if (isAttackClick)
            {
                AttackMoveWithMouseClick();
            }
            else if (isMoveClick)
            {
                MoveWithMouseClick();
            }
            else if (isPatrolClick)
            {
                PatrolWithMouseClick();
            }
            else if (isRallyPointClick)
            {
                SetRallyPoint();
            }
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
                ArrayMainbaseCommand.Use(EMainbaseCommnad.CANCLE);
                return;
            }

            if (SelectableObjectManager.GetFirstSelectedObjectInList().GetObjectType().Equals(EObjectType.BARRACK))
            {
                if (isRallyPointClick)
                    ClearCurFunc();
            }
            
            if (SelectableObjectManager.GetFirstSelectedObjectInList().GetUnitType.Equals(EUnitType.NONE))
            {
                return;
            }

            if (isAttackClick || isMoveClick || isPatrolClick || isRallyPointClick || isLaunchNuclearClick)
                ClearCurFunc();
            else
                MoveWithMouseClick();
        }
    }

    private void CheckIsHotkey()
    {
        if (SelectableObjectManager.IsListEmpty) return;

        EObjectType objType = SelectableObjectManager.GetFirstSelectedObjectInList().GetObjectType();
        switch (objType)
        {
            case EObjectType.UNIT_01:
                if (UnitDefaultHotkeyAction())
                    break;
                break;
            case EObjectType.UNIT_02:
                if (UnitDefaultHotkeyAction())
                    break;
                break;
            case EObjectType.UNIT_HERO:
                if (UnitDefaultHotkeyAction())
                    break;
                if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.LAUNCH_NUCLEAR]))
                    ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.LAUNCH_NUCLEAR);
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
                else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.WALL]))
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
            default:
                break;
        }
    }

    private bool StructureDefaultHotkeyAction()
    {
        if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.DEMOLISH]))
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.DEMOLISH);
        else if (Input.GetKeyDown(arrStructureFuncHotkey[(int)EStructureFuncHotkey.UPGRADE]))
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.UPGRADE);
        else if (Input.GetKeyDown(cancleKey))
            ArrayStructureFuncButtonCommand.Use(EStructureButtonCommand.CANCLE_CURRENT_FUNCTION);
        else
            return false;
        return true;
    }

    
    private bool UnitDefaultHotkeyAction()
    {
        if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.MOVE]))
            ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.MOVE);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.STOP]))
            ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.STOP);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.HOLD]))
            ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.HOLD);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.PATROL]))
            ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.PATROL);
        else if (Input.GetKeyDown(arrUnitFuncHotkey[(int)EUnitFuncHotkey.ATTACK]))
            ArrayUnitFuncButtonCommand.Use(EUnitFuncButtonCommand.ATTACK);
        else
            return false;

        return true;
    }
    

    private bool MainbaseBuildHotkeyAction()
    {
        if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.TURRET]))
            ArrayMainbaseCommand.Use(EMainbaseCommnad.BUILD_STRUCTURE, EObjectType.TURRET);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.BUNKER]))
            ArrayMainbaseCommand.Use(EMainbaseCommnad.BUILD_STRUCTURE, EObjectType.BUNKER);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.BARRACK]))
            ArrayMainbaseCommand.Use(EMainbaseCommnad.BUILD_STRUCTURE, EObjectType.BARRACK);
        else if (Input.GetKeyDown(arrBuildFuncHotkey[(int)EBuildFuncHotkey.NUCLEAR]))
            ArrayMainbaseCommand.Use(EMainbaseCommnad.BUILD_STRUCTURE, EObjectType.NUCLEAR);
        else
            return false;
        return true;
    }

    private void LateUpdate()
    {
        if (isPause) return;

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
        {
            ArrayUnitActionCommand.Use(EUnitActionCommand.FOLLOW_OBJECT, hit.transform);
        }
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

            if (isCheckDoubleClick)
            {
                if (hit.transform.Equals(SelectableObjectManager.GetFirstSelectedObjectInList().transform))
                {
                    Debug.Log("double Click");
                    // 여기서 카메라 커맨드로 박스캐스트나 오버랩 박스로 화면내의 selectable다 찾아내고
                    // 그 배열 받아와서 그 배열에 있는 애들 중 hit랑 타입 같은 애들만 골라서 temp에 일일이 다 넣어주기
                }

                isCheckDoubleClick = false;
            }


            if (sObj != null)
                ArraySelectCommand.Use(ESelectCommand.TEMP_SELECT, sObj);
        }

        Functions.Picking("StageFloor", floorLayer, ref dragStartPos);
        selectArea.SetPos(dragStartPos);
        selectArea.SetLocalScale(Vector3.zero);
        selectArea.SetActive(true);
        isCheckDoubleClick = true;

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

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
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
    private float leftClickElapsedTime = 0f;

    private bool isMoveClick = false;
    private bool isAttackClick = false;
    private bool isPatrolClick = false;
    private bool isBuildOperation = false;
    private bool isRallyPointClick = false;
    private bool isLaunchNuclearClick = false;
    private bool isCheckDoubleClick = false;
    private bool isPause = false;

    private GameObject pickPosDisplayGo = null;

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragEndPos = Vector3.zero;

    private SelectArea selectArea = null;
    
    private EObjectType objectType;

    private enum EUnitFuncHotkey { NONE = -1, MOVE, STOP, HOLD, PATROL, ATTACK, LAUNCH_NUCLEAR, LENGTH }
    private enum EBuildFuncHotkey { NONE = -1, TURRET, BUNKER, BARRACK, NUCLEAR, WALL, LENGTH }
    private enum EBarrackFuncHotkey { NONE = -1, SPAWN_MELEE, SPAWN_RANGED, SET_RALLYPOINT, UPGRADE_RANGED_DMG, UPGRADE_RANGED_HP, UPGRADE_MELEE_DMG, UPGRADE_MELEE_HP, LENGTH }
    private enum EStructureFuncHotkey { NONE = -1, UPGRADE, DEMOLISH, SPAWN_NUCLEAR, OUT_ONE_UNIT, OUT_ALL_UNIT, UPGRADE_ENERGY_SUPPLY, UPGRADE_POPULATION_MAX, LENGTH }
}