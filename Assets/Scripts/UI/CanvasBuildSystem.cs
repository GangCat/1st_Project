using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBuildSystem : MonoBehaviour
{
    public void Init(VoidIntDelegate _buildBtnCallback)
    {
        btnBuildTurret.onClick.AddListener(
            () =>
            {
                _buildBtnCallback?.Invoke((int)EBuildingType.Turret);
            });

        btnBuildBunker.onClick.AddListener(
            () =>
            {
                _buildBtnCallback?.Invoke((int)EBuildingType.Bunker);
            });

        btnBuildWall.onClick.AddListener(
            () =>
            {
                _buildBtnCallback?.Invoke((int)EBuildingType.Wall);
            });
    }



    [SerializeField]
    private Button btnBuildTurret = null;
    [SerializeField]
    private Button btnBuildBunker = null;
    [SerializeField]
    private Button btnBuildWall = null;
}
