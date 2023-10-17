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
        imageProgressbar.Init();
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

    public void UpdateSpawnList(List<EUnitType> _listUnit)
    {
        int i = 0;

        for (; i < _listUnit.Count; ++i)
            arrImageModel[i].ChangeSprite(arrUnitSprite[(int)_listUnit[i]]);

        for(; i < arrImageModel.Length; ++i)
            arrImageModel[i].Clear();
    }

    public void UpdateTime(float _percent)
    {
        imageProgressbar.UpdateLength(_percent);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    [Header("-Melee/Ranged/Rocket")]
    [SerializeField]
    private Sprite[] arrUnitSprite = null;
    [SerializeField]
    private ImageProgressbar imageProgressbar = null;

    private ImageModelSpawnQueue[] arrImageModel = null;
}
