using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBuildFunc : CanvasFunc, ISubscriber
{
    public void Init()
    {
        arrBuildFuncBtn = new FuncButtonBase[(int)EBuildFuncKey.LENGTH];

        arrBuildFuncBtn[0] = btnBuildTurret;
        arrBuildFuncBtn[1] = btnBuildBunker;
        arrBuildFuncBtn[2] = btnBuildBarrack;
        arrBuildFuncBtn[3] = btnBuildNuclear;
        arrBuildFuncBtn[4] = btnBuildWall;

        for(int i = 0; i  < arrBuildFuncBtn.Length; ++i)
        {
            arrBuildFuncBtn[i].Init();
        }

        arrBuildFuncBtn[4].SetActive(false);

        gameObject.SetActive(false);
    }

    public void DisplayBuildWallFunc()
    {
        SetActive(true);
        arrBuildFuncBtn[(int)EBuildFuncKey.TURRET].SetActive(false);
        arrBuildFuncBtn[(int)EBuildFuncKey.BUNKER].SetActive(false);
        arrBuildFuncBtn[(int)EBuildFuncKey.BARRACK].SetActive(false);
        arrBuildFuncBtn[(int)EBuildFuncKey.NUCLEAR].SetActive(false);
        arrBuildFuncBtn[(int)EBuildFuncKey.WALL].SetActive(true);
    }

    public void HideBuildWallFunc()
    {
        arrBuildFuncBtn[(int)EBuildFuncKey.TURRET].SetActive(true);
        arrBuildFuncBtn[(int)EBuildFuncKey.BUNKER].SetActive(true);
        arrBuildFuncBtn[(int)EBuildFuncKey.BARRACK].SetActive(true);
        arrBuildFuncBtn[(int)EBuildFuncKey.NUCLEAR].SetActive(true);
        arrBuildFuncBtn[(int)EBuildFuncKey.WALL].SetActive(false);
        SetActive(false);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.ENERGY_UPDATE);
    }

    public void ReceiveMessage(EMessageType _message)
    {

    }

    [SerializeField]
    private ButtonBuildBarrack btnBuildBarrack = null;
    [SerializeField]
    private ButtonBuildBunker btnBuildBunker = null;
    [SerializeField]
    private ButtonBuildNuclear btnBuildNuclear = null;
    [SerializeField]
    private ButtonBuildTurret btnBuildTurret = null;
    [SerializeField]
    private ButtonBuildWall btnBuildWall = null;

    private FuncButtonBase[] arrBuildFuncBtn = null;
}
