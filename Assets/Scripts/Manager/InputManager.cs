using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public void Init(
        VoidVec3Delegate _pickingCallback,
        VoidFloatDelegate _cameraZoomCallback,
        VoidVec2Delegate _moveCameraWithMouseCallback,
        VoidVec2Delegate _moveCameraWithKeyCallback)
    {
        pickingCallback = _pickingCallback;
        cameraZoomCallback = _cameraZoomCallback;
        moveCameraWithMouseCallback = _moveCameraWithMouseCallback;
        moveCameraWithKeyCallback = _moveCameraWithKeyCallback;

        selectArea = GetComponentInChildren<SelectArea>();
        selectArea.Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SelectOperateWithMouseLeftClick();
        else if (Input.GetMouseButtonDown(1))
            MoveOperateWithMouseRightClick();
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
    }

    private void MoveOperateWithMouseRightClick()
    {
        pickPos = Vector3.zero;
        if (Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref pickPos))
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            pickingCallback?.Invoke(pickPos);
        }

    }

    private void SelectOperateWithMouseLeftClick()
    {
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
            selectArea.SetLocalScale(Quaternion.Euler(0f, -45f, 0f) * dragEndPos - Quaternion.Euler(0f, -45f, 0f) * dragStartPos);
            
            // 언제 한 번씩 picking할지 결정하기
            yield return null;
        }

        selectArea.SetActive(false);
    }

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    private void ZoomCamera()
    {
        cameraZoomCallback?.Invoke(Input.GetAxisRaw("Mouse ScrollWheel"));
    }

    private void MoveCamera()
    {
        if (Input.GetAxisRaw("Horizontal Arrow").Equals(0) && Input.GetAxisRaw("Vertical Arrow").Equals(0))
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

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 dragEndPos = Vector3.zero;
    private Vector3 pickPos = Vector3.zero;

    private List<SelectableObject> selectedObjects = new List<SelectableObject>();

    private SelectArea selectArea = null;

    private VoidVec3Delegate pickingCallback = null;
    private VoidFloatDelegate cameraZoomCallback = null;
    private VoidVec2Delegate moveCameraWithMouseCallback = null;
    private VoidVec2Delegate moveCameraWithKeyCallback = null;

}