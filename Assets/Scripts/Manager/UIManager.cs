using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init(VoidIntDelegate _buildBtnCallback)
    {
        canvasFuncButton = GetComponentInChildren<CanvasFuncButton>();
        canvasFuncButton.Init(_buildBtnCallback);
    }

    public void ShowFuncButton(ESelectableObjectType _selectObjectType)
    {
        canvasFuncButton.ShowFuncButton(_selectObjectType);
    }

    private CanvasFuncButton canvasFuncButton = null;
}
