using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDebugMode : MonoBehaviour
{
    public void Init()
    {
        textCurState = GetComponentInChildren<TextCurStateIndicator>();
        textCurState.Init();
    }

    public void DisplayCurState(Vector3 _screenPos, EState _curState)
    {
        textCurState.SetPos(_screenPos.x + 25f, _screenPos.y + 50f, _screenPos.z);
        textCurState.UpdateText(Enum.GetName(typeof(EState), _curState));
    }

    private static TextCurStateIndicator textCurState = null;
}
