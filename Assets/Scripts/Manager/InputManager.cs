using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public void Init(VoidVec3Delegate _pickingCallback)
    {
        pickingCallback = _pickingCallback;
    }

    private void Update()
    {
        UpdateMouseInput();
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

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    [SerializeField]
    private GameObject pickPosPrefab = null;
    [SerializeField]
    private float pickPosDisplayHideDelay = 0.3f;

    private VoidVec3Delegate pickingCallback = null;
    private Vector3 pickPos = Vector3.zero;
}
