using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public void Init(
        VoidVec3Delegate _pickingCallback,
        VoidTransformDelegate _PickingObjectCallback,
        VoidFloatDelegate _cameraZoomCallback,
        VoidVec2Delegate _moveCameraWithMouseCallback,
        VoidVec2Delegate _moveCameraWithKeyCallback,
        VoidTemplateDelegate<SelectableObject> _selectObjectCallback,
        VoidTemplateDelegate<SelectableObject> _unSelectObjectCallback,
        VoidVoidDelegate _selectFinishCallback,
        VoidVoidDelegate _moveCameraWithObjectCallback,
        VoidVec3Delegate _attackMoveCallback,
        VoidVec3Delegate _patrolCallback)
    {
        pickingCallback = _pickingCallback;
        PickingObjectCallback = _PickingObjectCallback;
        cameraZoomCallback = _cameraZoomCallback;
        moveCameraWithMouseCallback = _moveCameraWithMouseCallback;
        moveCameraWithKeyCallback = _moveCameraWithKeyCallback;
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

    private void ClearCurFunc()
    {
        isMoveClick = false;
        isAttackClick = false;
        isPatrolClick = false;
        isRallyPointClick = false;
        isBuildOperation = false;
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
        ArrayBarrackCommand.Use(EBarrackCommand.RALLYPOINT_CONFIRM);

        Vector3 pickPos = Vector3.zero;
        Functions.Picking("StageFloor", floorLayer, ref pickPos);
        GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
        StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);

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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        if (Functions.Picking(out hit))
        {
            if(hit.transform.GetComponent<SelectableObject>())
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
        cameraZoomCallback?.Invoke(Input.GetAxisRaw("Mouse ScrollWheel"));
    }

    private void MoveCamera()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            moveCameraWithObjectCallback?.Invoke();
        }
        else if (Input.GetAxisRaw("Horizontal Arrow").Equals(0) && Input.GetAxisRaw("Vertical Arrow").Equals(0))
            moveCameraWithMouseCallback?.Invoke(Input.mousePosition);
        else
            moveCameraWithKeyCallback?.Invoke
                (
                new Vector2
                    (
                    Input.GetAxisRaw("Horizontal Arrow"),
                    Input.GetAxisRaw("Vertical Arrow")
                    )
                );
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

    private GameObject pickPosDisplayGo = null;

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragEndPos = Vector3.zero;

    private SelectArea selectArea = null;

    private VoidVec3Delegate pickingCallback = null;
    private VoidTransformDelegate PickingObjectCallback = null;
    private VoidFloatDelegate cameraZoomCallback = null;
    private VoidVec2Delegate moveCameraWithMouseCallback = null;
    private VoidVec2Delegate moveCameraWithKeyCallback = null;
    private VoidVoidDelegate selectFinishCallback = null;
    private VoidVoidDelegate moveCameraWithObjectCallback = null;
    private VoidTemplateDelegate<SelectableObject> selectObjectCallback = null;
    private VoidVec3Delegate attackMoveCallback = null;
    private VoidVec3Delegate patrolCallback = null;
}