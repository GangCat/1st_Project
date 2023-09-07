using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFuncButton : MonoBehaviour
{
    public void Init(VoidIntDelegate _buildBtnCallback)
    {
        //arrBtnBuildFunc[0].onClick.AddListener(
        //    () =>
        //    {
        //        _buildBtnCallback?.Invoke((int)EBuildingType.Turret);
        //    });

        //arrBtnBuildFunc[1].onClick.AddListener(
        //    () =>
        //    {
        //        _buildBtnCallback?.Invoke((int)EBuildingType.Bunker);
        //    });

        //arrBtnBuildFunc[2].onClick.AddListener(
        //    () =>
        //    {
        //        _buildBtnCallback?.Invoke((int)EBuildingType.Wall);
        //    });
    }

    public void ShowFuncButton(ESelectableObjectType _selectObjectType)
    {
        HideAllFuncButton();

        switch (_selectObjectType)
        {
            case ESelectableObjectType.FriendlyUnit:
                ShowFriendlyUnitFuncButton();
                break;
            case ESelectableObjectType.FriendlyStructure:
                ShowFriendlyStructureButton();
                break;
            case ESelectableObjectType.EnemyUnit:
            case ESelectableObjectType.EnemyStructure:
                ShowEnemyFuncButton();
                break;
        }
    }

    private void HideAllFuncButton()
    {
        switch (curActiveBtnFunc)
        {
            case EButtonFuncType.FriendlyUnit:
                foreach (Button btn in arrBtnUnitFunc)
                    btn.gameObject.SetActive(false);
                break;
            case EButtonFuncType.Structure:
                break;
            case EButtonFuncType.Enemy:
                break;
            case EButtonFuncType.Build:
                foreach (Button btn in arrBtnBuildFunc)
                    btn.gameObject.SetActive(false);
                break;
            case EButtonFuncType.Tower:
                break;
            case EButtonFuncType.Bunker:
                break;
            case EButtonFuncType.Hero:
                break;
        }
    }

    private void ShowFriendlyUnitFuncButton()
    {
        foreach (Button btn in arrBtnUnitFunc)
            btn.gameObject.SetActive(true);

        curActiveBtnFunc = EButtonFuncType.FriendlyUnit;
    }

    private void ShowFriendlyStructureButton()
    {
        foreach (Button btn in arrBtnBuildFunc)
            btn.gameObject.SetActive(true);

        curActiveBtnFunc = EButtonFuncType.Build;
    }

    private void ShowEnemyFuncButton()
    {
        Debug.Log("EnemyFunc");
    }

    [Header("-Friendly Unit Button")]
    [SerializeField]
    private Button[] arrBtnUnitFunc = null;

    [Header("-Build Structure Button")]
    [SerializeField]
    private Button[] arrBtnBuildFunc = null;

    private EButtonFuncType curActiveBtnFunc = EButtonFuncType.None;
}
