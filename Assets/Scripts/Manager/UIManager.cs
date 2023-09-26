using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init()
    {
        canvasFuncButton = GetComponentInChildren<FuncButtonManager>();
        canvasFuncButton.Init();
    }

    public void ShowFuncButton(EObjectType _selectObjectType)
    {
        canvasFuncButton.ShowFuncButton(_selectObjectType);
    }

    private FuncButtonManager canvasFuncButton = null;
}
