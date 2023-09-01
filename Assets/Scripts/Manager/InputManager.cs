using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public void Init(VoidVec3Delegate _pickingCallback, VoidFloatDelegate _cameraZoomCallback)
    {
        pickingCallback = _pickingCallback;
        cameraZoomCallback = _cameraZoomCallback;
    }

    

    private void Update()
    {
        UpdateMouseInput();
        UpdateCameraZoom();
    }

    private void LateUpdate()
    {
        
    }

    private void UpdateMouseInput()
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

    private void UpdateCameraZoom()
    {
        cameraZoomCallback?.Invoke(Input.GetAxisRaw("Mouse ScrollWheel"));
    }

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    [SerializeField]
    private GameObject pickPosPrefab = null;
    [SerializeField]
    private float pickPosDisplayHideDelay = 0.3f;

    private Vector3 pickPos = Vector3.zero;

    private VoidVec3Delegate pickingCallback = null;
    private VoidFloatDelegate cameraZoomCallback = null;


}
