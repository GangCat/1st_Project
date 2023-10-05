using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IMinimapObserver
{
    public void Init(
        VoidVec3Delegate _pickingCallback,
        VoidTransformDelegate _PickingObjectCallback,
        VoidTemplateDelegate<SelectableObject> _selectObjectCallback,
        VoidTemplateDelegate<SelectableObject> _unSelectObjectCallback,
        VoidVoidDelegate _selectFinishCallback,
        VoidVoidDelegate _moveCameraWithObjectCallback,
        VoidVec3Delegate _attackMoveCallback,
        VoidVec3Delegate _patrolCallback)
    {
        pickingCallback = _pickingCallback;
        PickingObjectCallback = _PickingObjectCallback;
        selectFinishCallback = _selectFinishCallback;
        moveCameraWithObjectCallback = _moveCameraWithObjectCallback;
        selectObjectCallback = _selectObjectCallback;
        attackMoveCallback = _attackMoveCallback;
        patrolCallback = _patrolCallback;

        selectArea = GetComponentInChildren<SelectArea>();
        selectArea.Init(_selectObjectCallback, _unSelectObjectCallback);
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
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            pickingCallback?.Invoke(pickPos);
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
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            attackMoveCallback?.Invoke(pickPos);
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
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", floorLayer, ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            patrolCallback?.Invoke(pickPos);
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

        if (Functions.Picking(selectableLayer, out hit))
        {
            if (hit.transform.GetComponent<SelectableObject>())
                selectObjectCallback?.Invoke(hit.transform.GetComponent<SelectableObject>());
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

        selectFinishCallback?.Invoke();
        selectArea.SetActive(false);
    }

    private void ZoomCamera()
    {
        ArrayCameraMoveCommand.Use(ECameraCommand.ZOOM, Input.GetAxisRaw("Mouse ScrollWheel"));
        //cameraZoomCallback?.Invoke(Input.GetAxisRaw("Mouse ScrollWheel"));
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            moveCameraWithObjectCallback?.Invoke();
        }
        else if (Input.GetAxisRaw("Horizontal Arrow").Equals(0) && Input.GetAxisRaw("Vertical Arrow").Equals(0))
            //moveCameraWithMouseCallback?.Invoke(Input.mousePosition);
            ArrayCameraMoveCommand.Use(ECameraCommand.MOVE_WITH_MOUSE, Input.mousePosition);
        else
            ArrayCameraMoveCommand.Use(
                ECameraCommand.MOVE_WITH_KEY,
                new Vector2(Input.GetAxisRaw("Horizontal Arrow"), Input.GetAxisRaw("Vertical Arrow"))
                );
    }

    public void GetUnitTargetPos(Vector3 _pos)
    {

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
    private KeyCode cancleKey = KeyCode.Escape;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private LayerMask selectableLayer;

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

    private VoidVec3Delegate pickingCallback = null;
    private VoidTransformDelegate PickingObjectCallback = null;
    private VoidVoidDelegate selectFinishCallback = null;
    private VoidVoidDelegate moveCameraWithObjectCallback = null;
    private VoidTemplateDelegate<SelectableObject> selectObjectCallback = null;
    private VoidVec3Delegate attackMoveCallback = null;
    private VoidVec3Delegate patrolCallback = null;
}