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
        VoidVoidDelegate _moveCameraWithObjectCallback)
    {
        pickingCallback = _pickingCallback;
        PickingObjectCallback = _PickingObjectCallback;
        cameraZoomCallback = _cameraZoomCallback;
        moveCameraWithMouseCallback = _moveCameraWithMouseCallback;
        moveCameraWithKeyCallback = _moveCameraWithKeyCallback;
        selectFinishCallback = _selectFinishCallback;
        moveCameraWithObjectCallback = _moveCameraWithObjectCallback;
        selectObjectCallback = _selectObjectCallback;

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

    public void OnClickCancleButton()
    {
        ClearCurFunc();
    }

    private void ClearCurFunc()
    {
        isMoveClick = false;
        // ��� ��ɰ� ���õ� bool�� ��� �ʱ�ȭ
    }


    private void Update()
    {
        if (isMoveClick)
        {
            RaycastHit hit;
            if(pickPosDisplayGo != null && Functions.Picking(out hit))
                pickPosDisplayGo.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(pickPosDisplayGo);
                MoveOperateWithMouseClick();
            }
        }
        else if (IsBuildOperation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                ListBuildCommand.Use(1);
            }
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                ListBuildCommand.Use(0);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                SelectOperateWithMouseClick();
            else if (Input.GetMouseButtonDown(1))
                MoveOperateWithMouseClick();
        }
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
    }

    private void MoveOperateWithMouseClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (isMoveClick) isMoveClick = false;

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

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    private void SelectOperateWithMouseClick()
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

    private bool isMoveClick = false;

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
}