using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init(
        VoidVoidDelegate _moveBtnCallback,
        VoidVoidDelegate _cancleBtnCallback)
    {
        canvasFuncButton = GetComponentInChildren<FuncButtonManager>();
        canvasFuncButton.Init(_moveBtnCallback, _cancleBtnCallback);
    }

    public void ShowFuncButton(ESelectableObjectType _selectObjectType)
    {
        canvasFuncButton.ShowFuncButton(_selectObjectType);
    }

    private FuncButtonManager canvasFuncButton = null;
}
