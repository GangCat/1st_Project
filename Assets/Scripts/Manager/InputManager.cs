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

    public bool IsMoveClick { get; set; }
    public bool IsBuildOperation { get; set; }
    
    public Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnClickMoveButton()
    {
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isMoveClick = true;
    }

    public void OnClickAttackButton()
    {
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isAttackClick = true;
    }

    public void OnClickPatrolButton()
    {
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isPatrolClick = true;
    }

    public void OnClickRallyPointButton()
    {
        pickPosDisplayGo = Instantiate(pickPosPrefab, transform);
        isRallyPointClick = true;
    }

    public void OnClickCancleButton()
    {
        ClearCurFunc();
    }


    private void ClearCurFunc()
    {
        isMoveClick = false;
        isAttackClick = false;
        isPatrolClick = false;
        // 등등 기능과 관련된 bool값 모두 초기화
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;



        if (isAttackClick)
        {
            RaycastHit hit;
            if (pickPosDisplayGo != null && Functions.Picking(out hit))
                pickPosDisplayGo.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
                AttackMoveWithMouseClick();

            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
            }
        }
        else if (isMoveClick)
        {
            RaycastHit hit;
            if(pickPosDisplayGo != null && Functions.Picking(out hit))
                pickPosDisplayGo.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(pickPosDisplayGo);
                OnClickCancleButton();
                MoveWithMouseClick();
            }
            else if(Input.GetMouseButtonDown(1))
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
            }
        }
        else if (isPatrolClick)
        {
            RaycastHit hit;
            if (pickPosDisplayGo != null && Functions.Picking(out hit))
                pickPosDisplayGo.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
                PatrolWithMouseClick();
                
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(pickPosDisplayGo);
                ClearCurFunc();
            }
        }
        else if (IsBuildOperation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                ArrayBuildCommand.Use(EMainStructureCommnad.CONFIRM);
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                ArrayBuildCommand.Use(EMainStructureCommnad.CANCLE);
        }
        else if (isRallyPointClick)
        {

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                DragOperateWithMouseClick();
            else if (Input.GetMouseButtonDown(1))
            {
                MoveWithMouseClick();
            }
        }
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
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
        if (Functions.Picking(1 << LayerMask.NameToLayer("SelectableObject"), out hit))
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            pickingCallback?.Invoke(pickPos);
        }
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
        if (Functions.Picking(1 << LayerMask.NameToLayer("SelectableObject"), out hit))
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            attackMoveCallback?.Invoke(pickPos);
        }
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
        if (Functions.Picking(1 << LayerMask.NameToLayer("SelectableObject"), out hit))
            PickingObjectCallback?.Invoke(hit.transform);
        else if (Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            patrolCallback?.Invoke(pickPos);
        }
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

        Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref dragStartPos);
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

            Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref dragEndPos);
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

    private float elapsedTime = 0f;

    private bool isMoveClick = false;
    private bool isAttackClick = false;
    private bool isPatrolClick = false;
    private bool isRallyPointClick = false;

    private GameObject pickPosDisplayGo;

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