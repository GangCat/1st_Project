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
        pathMng = FindAnyObjectByType<PF_PathRequestManager>();
    }

    private void Start()
    {
        // 마우스 가두기
        Cursor.lockState = CursorLockMode.Confined;

        ListUnitCommand.Add(new CommandCancle());
        ListUnitCommand.Add(new CommandMove(inputMng));
        ListUnitCommand.Add(new CommandStop(selectMng));

        ListBuildCommand.Add(new CommandBuildCancle(buildMng, inputMng));
        ListBuildCommand.Add(new CommandBuildConfirm(buildMng, inputMng));
        ListBuildCommand.Add(new CommandBuildTurret(buildMng, inputMng));
        ListBuildCommand.Add(new CommandBuildBunker(buildMng, inputMng));
        ListBuildCommand.Add(new CommandBuildWall(buildMng, inputMng));
        ListBuildCommand.Add(new CommandBuildNuclear(buildMng, inputMng));


        grid = pathMng.GetComponent<PF_Grid>();

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
        uiMng.Init();
        buildMng.Init(grid);
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

            // 여기서 각 unit의 pathrequest를 진행
            
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

    public void OnClickMoveButton()
    {
        inputMng.OnClickMoveButton();
    }

    private void BuildButtonOnClick(int _buildingType)
    {
        buildMng.ShowBluepirnt((ESelectableObjectType)_buildingType);
    }




    private InputManager inputMng = null;
    private CameraManager cameraMng = null;
    private SelectableObjectManager selectMng = null;
    private UIManager uiMng = null;
    private BuildManager buildMng = null;
    private PF_PathRequestManager pathMng = null;

    private PF_Grid grid = null;
}
