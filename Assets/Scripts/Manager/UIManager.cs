using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void Init(VoidIntDelegate _buildBtnCallback)
    {
        canvasBuildSys = GetComponentInChildren<CanvasBuildSystem>();
        canvasBuildSys.Init(_buildBtnCallback);
    }


    private CanvasBuildSystem canvasBuildSys = null;
}
