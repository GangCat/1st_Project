using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSpawnUnitInfo : MonoBehaviour
{
    public void Init()
    {
        arrImageModel = GetComponentsInChildren<ImageModelSpawnQueue>();
        foreach (ImageModelSpawnQueue image in arrImageModel)
            image.Init();
        imageSpawnProgressbar.Init();
        SetActive(false);
    }

    public void HideDisplay()
    {
        SetActive(false);
    }

    public void ShowDisplay()
    {
        SetActive(true);
    }

    public void AddSpawnQueue(EUnitType _unitType)
    {
        SetActive(true);
        arrImageModel[curQueueCnt].ChangeSprite(arrUnitSprite[(int)_unitType]);
        ++curQueueCnt;
    }

    public void SpawnFinish()
    {
        --curQueueCnt;
        if(curQueueCnt < 1)
        {
            SetActive(false);
            return;
        }

        int i = 0;
        for (; i < curQueueCnt; ++i)
            arrImageModel[i].ChangeSprite(arrImageModel[i + 1].GetCurSprite());
        for (; i < arrImageModel.Length; ++i)
            arrImageModel[i].Clear();
    }

    public void Updateprogress(float _progressPercent)
    {
        imageSpawnProgressbar.UpdateLength(_progressPercent);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    [Header("-Melee/Ranged/Rocket")]
    [SerializeField]
    private Sprite[] arrUnitSprite = null;
    [SerializeField]
    private ImageProgressbar imageSpawnProgressbar = null;

    private ImageModelSpawnQueue[] arrImageModel = null;
    private int curQueueCnt = 0;
}
