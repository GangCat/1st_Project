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
            MovePlayerByPicking, 
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
        selectMng.MoveUnitByPicking(_pickPos);
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
        // �̵��� �ش� �������� �߽������� �� ������Ʈ���� ��ġ�� ������. �� �� ��ư�����ó�� ���������� �ؼ� 3by4��
        // /�� % �̿��ؼ� ���ϰ� ����
        // ���õ� ������Ʈ �� �ؿ� �� ����
        // ��ü �̵�
    }

    private PlayerManager playerMng = null;
    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
}
