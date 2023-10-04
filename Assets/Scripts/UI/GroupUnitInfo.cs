using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupUnitInfo : MonoBehaviour
{
    public void Init()
    {
        arrImageUnit = GetComponentsInChildren<ImageFriendlyUnit>();
        SetActive(false);
    }

    public void InitList(List<SFriendlyUnitInfo> _list)
    {
        listFriendlyUnitInfo = _list;
        for(int i = 0; i < arrImageUnit.Length; ++i)
        {
            SFriendlyUnitInfo tempInfo = listFriendlyUnitInfo[i];
            arrImageUnit[i].Init(ref tempInfo.curHpPercent);
        }
    }

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void DisplayGroupInfo(int _unitCnt)
    {
        foreach (ImageFriendlyUnit image in arrImageUnit)
            image.SetActive(false);

        unitCnt = _unitCnt;
        SetActive(true);
        StartCoroutine("UpdateHpDisplayCoroutine");

        for(int i = 0; i < _unitCnt; ++i)
        {
            arrImageUnit[i].SetActive(true);
            switch (listFriendlyUnitInfo[i].unitType)
            {
                case EUnitType.MELEE:
                    arrImageUnit[i].ChangeSprite(arrFriendlyUnitSprite[0]);
                    break;
                case EUnitType.RANGED:
                    arrImageUnit[i].ChangeSprite(arrFriendlyUnitSprite[1]);
                    break;
                case EUnitType.HERO:
                    arrImageUnit[i].ChangeSprite(arrFriendlyUnitSprite[2]);
                    break;
                default:
                    break;
            }
            Debug.Log(listFriendlyUnitInfo[i].curHpPercent);
        }
    }

    private IEnumerator UpdateHpDisplayCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < unitCnt; ++i)
                arrImageUnit[i].updateHpDisplay(listFriendlyUnitInfo[i].curHpPercent);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void HideDisplay()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private ImageFriendlyUnit[] arrImageUnit = null;
    private List<SFriendlyUnitInfo> listFriendlyUnitInfo = null;

    private int unitCnt = 0;

    [Header("-Melee, Ranged, Hero")]
    [SerializeField]
    private Sprite[] arrFriendlyUnitSprite = null;
}
