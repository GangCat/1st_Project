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
        // ���콺 ���α�
        Cursor.lockState = CursorLockMode.Confined;

        selectMng.Init();
        inputMng.Init(
            MoveUnitByPicking,
            MoveUnitByPickingObject,
            ZoomCamera, 
            MoveCameraWithMouse, 
            MoveCameraWithKey, 
            AddSelectedObject, 
            RemoveSelectedObject, 
            SelectFinish,
            MoveCameraWithObject);
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
            selectMng.MoveUnit(_pickPos);
        }
    }

    private void MoveUnitByPickingObject(Transform _targetTr)
    {
        if (selectMng.IsListEmpty) return;

        if (selectMng.IsFriendlyUnit)
        {
            selectMng.MoveUnitByPicking(_targetTr);
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

    private void MoveCameraWithObject()
    {
        cameraMng.MoveCameraWithObject(selectMng.GetFirstSelectableObjectInList.GetPos);
    }

    private void AddSelectedObject(SelectableObject _object)
    {
        selectMng.AddSelectedObject(_object);
        // ���õ� obj �ƿ����� ǥ��
    }

    private void RemoveSelectedObject(SelectableObject _object)
    {
        selectMng.RemoveSelectedObject(_object);
        // �ش� obj �ƿ����� ����
    }

    private void SelectFinish()
    {
        // ��ȯ�Ǵ� �迭�� ������ �ش� �迭�� ������Ʈ���� �� �ؿ� �ձ� ���� ����
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
