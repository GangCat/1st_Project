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
    }

    private void Update()
    {
        MoveOperateWithMouseRightClick();
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
    }

    private void MoveOperateWithMouseRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pickPos = Vector3.zero;
            if (Functions.Picking("StageFloor", 1 << LayerMask.NameToLayer("StageFloor"), ref pickPos))
            {
                GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, pickPos, Quaternion.identity, transform);
                StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
                pickingCallback?.Invoke(pickPos);
            }
        }
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
        if(Input.GetAxisRaw("Horizontal Arrow").Equals(0) && Input.GetAxisRaw("Vertical Arrow").Equals(0))
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

    private Vector3 pickPos = Vector3.zero;

    private VoidVec3Delegate pickingCallback = null;
    private VoidFloatDelegate cameraZoomCallback = null;
    private VoidVec2Delegate moveCameraWithMouseCallback = null;
    private VoidVec2Delegate moveCameraWithKeyCallback = null;

}