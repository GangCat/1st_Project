using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUpgradeInfo : MonoBehaviour
{
    public void Init()
    {
        imageUpgradeModel = GetComponentInChildren<ImageModel>();
        imageUpgradeModel.Init();
        imageUpgradeProgressbar.Init();
        SetActive(false);
    }

    public void HideDisplay()
    {
        SetActive(false);
    }

    private void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void UpgradeFinish()
    {
        gameObject.SetActive(false);
    }

    public void DisplayUpgradeInfo(EUpgradeType _type)
    {
        gameObject.SetActive(true);
        imageUpgradeModel.ChangeSprite(arrSpriteUpgrade[(int)_type]);
    }

    public void UpdateUpgradeProgress(float _progressPercent)
    {
        imageUpgradeProgressbar.UpdateLength(_progressPercent);
    }

    [SerializeField]
    private ImageProgressbar imageUpgradeProgressbar = null;
    [Header("-Population/Energy/Structure/RangedDmg/RangedHp/MeleeDmg/MeleeHp")]
    [SerializeField]
    private Sprite[] arrSpriteUpgrade = null;


    private ImageModel imageUpgradeModel = null;
}
