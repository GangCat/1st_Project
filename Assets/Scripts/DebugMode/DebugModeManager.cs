using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeManager : MonoBehaviour
{
    private void Awake()
    {
        canvasDebug = GetComponentInChildren<CanvasDebugMode>();
        canvasDebug.Init();
    }

    public static void DisplayCurState(Vector3 _screenPos, EState _curState)
    {
        canvasDebug.DisplayCurState(_screenPos, _curState);
    }

    private static CanvasDebugMode canvasDebug = null;
}
