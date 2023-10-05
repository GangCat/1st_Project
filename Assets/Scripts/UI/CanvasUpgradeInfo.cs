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

    public void UpgradeMainbase(EUpgradeETCType _upgradeType)
    {
        gameObject.SetActive(true);
        imageUpgradeModel.ChangeSprite(arrSpriteMainbaseupgrade[(int)_upgradeType]);
    }

    public void UpgradeUnit(EUnitUpgradeType _upgradetype)
    {
        gameObject.SetActive(true);
        imageUpgradeModel.ChangeSprite(arrSpriteUnitUpgrade[(int)_upgradetype]);
    }

    public void UpgradeStructure()
    {
        gameObject.SetActive(true);
        imageUpgradeModel.ChangeSprite(spriteStructureUpgrade);
    }

    public void UpgradeFinish()
    {
        gameObject.SetActive(false);
    }

    public void DisplayUpgradeInfo()
    {
        gameObject.SetActive(true);
    }

    public void UpdateUpgradeProgress(float _progressPercent)
    {
        imageUpgradeProgressbar.UpdateLength(_progressPercent);
    }

    [SerializeField]
    private ImageProgressbar imageUpgradeProgressbar = null;

    [SerializeField]
    private Sprite spriteStructureUpgrade = null;
    [Header("-Population/Energy")]
    [SerializeField]
    private Sprite[] arrSpriteMainbaseupgrade = null;
    [Header("-RD/RH/MD/MH")]
    [SerializeField]
    private Sprite[] arrSpriteUnitUpgrade = null;

    private ImageModel imageUpgradeModel = null;
}
