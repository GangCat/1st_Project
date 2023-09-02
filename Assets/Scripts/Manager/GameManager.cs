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
        // 이동시 해당 도착점을 중심점으로 각 오브젝트들의 위치를 결정함. 이 때 버튼만든것처럼 오프셋으로 해서 3by4로
        // /랑 % 이용해서 편하게 ㅇㅇ
        // 선택된 오브젝트 발 밑에 원 생성
        // 단체 이동
    }

    private PlayerManager playerMng = null;
    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
}
