using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        inputMng = FindAnyObjectByType<InputManager>();
        playerMng = FindAnyObjectByType<PlayerManager>();
        cameraMng = FindAnyObjectByType<CameraManager>();
        selectMng = FindAnyObjectByType<SelectableObjectManager>();
    }

    private void Start()
    {
        // 마우스 가두기
        Cursor.lockState = CursorLockMode.Confined;

        selectMng.Init();
        inputMng.Init(
            MoveUnitByPicking, 
            ZoomCamera, 
            MoveCameraWithMouse, 
            MoveCameraWithKey, 
            AddSelectedObject, 
            RemoveSelectedObject, 
            SelectFinish);
        playerMng.Init();
        cameraMng.Init();
    }

    private void MovePlayerByPicking(Vector3 _pickPos)
    {
        playerMng.MovePlayerByPicking(_pickPos);
    }

    private void MoveUnitByPicking(Vector3 _pickPos)
    {
        if (selectMng.IsListEmpty) return;

        if (selectMng.IsFriendlyUnit)
        {
            GameObject pickPosDisplayGo = Instantiate(pickPosPrefab, _pickPos, Quaternion.identity, transform);
            StartCoroutine("DestroypickPosDisplay", pickPosDisplayGo);
            selectMng.MoveUnitByPicking(_pickPos);
        }
    }

    private IEnumerator DestroypickPosDisplay(GameObject _go)
    {
        yield return new WaitForSeconds(pickPosDisplayHideDelay);
        Destroy(_go);
    }

    private void ZoomCamera(float _zoomRatio)
    {
        cameraMng.ZoomCamera(_zoomRatio);
    }

    private void MoveCameraWithMouse(Vector2 _mousePos)
    {
        cameraMng.MoveCameraWithMouse(_mousePos);
    }

    private void MoveCameraWithKey(Vector2 _arrowKeyInput)
    {
        cameraMng.MoveCameraWithKey(_arrowKeyInput);
    }

    private void AddSelectedObject(SelectableObject _object)
    {
        selectMng.AddSelectedObject(_object);
        // 선택된 obj 아웃라인 표시
    }

    private void RemoveSelectedObject(SelectableObject _object)
    {
        selectMng.RemoveSelectedObject(_object);
        // 해당 obj 아웃라인 해제
    }

    private void SelectFinish()
    {
        // 반환되는 배열을 가지고 해당 배열의 오브젝트들의 발 밑에 둥근 원을 생성
        selectMng.SelectFinish();
    }

    [SerializeField]
    private GameObject pickPosPrefab = null;
    [SerializeField]
    private float pickPosDisplayHideDelay = 0.3f;

    private PlayerManager playerMng = null;
    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
}
