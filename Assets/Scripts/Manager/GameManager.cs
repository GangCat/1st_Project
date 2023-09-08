using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        inputMng = FindAnyObjectByType<InputManager>();
        cameraMng = FindAnyObjectByType<CameraManager>();
        selectMng = FindAnyObjectByType<SelectableObjectManager>();
        uiMng = FindAnyObjectByType<UIManager>();
        buildMng = FindAnyObjectByType<BuildManager>();
    }

    private void Start()
    {
        // ���콺 ���α�
        Cursor.lockState = CursorLockMode.Confined;

        selectMng.Init(UnitSelect);
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
        cameraMng.Init();

        uiMng.Init(
            OnClickMoveButton,
            OnClickCancleButton);
    }

    private void UnitSelect(ESelectableObjectType _selectObjectType)
    {
        uiMng.ShowFuncButton(_selectObjectType);
    }

    private void MoveUnitByPicking(Vector3 _pickPos)
    {
        if (selectMng.IsListEmpty) return;

        if (selectMng.IsFriendlyUnit)
        {

            // ���⼭ �� unit�� pathrequest�� ����
            
            selectMng.MoveUnitByPicking(_pickPos);
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

    private void OnClickMoveButton()
    {
        inputMng.OnClickMoveButton();
    }

    private void OnClickCancleButton()
    {
        inputMng.OnClickCancleButton();
    }

    private void BuildButtonOnClick(int _buildingType)
    {
        buildMng.ShowBlutpirnt((ESelectableObjectType)_buildingType);
    }




    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
    private UIManager uiMng = null;
    private BuildManager buildMng = null;
    private VoidVoidDelegate onClickMoveBtnCallback = null;

}
