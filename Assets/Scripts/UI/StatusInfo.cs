using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusInfo : MonoBehaviour
{
    public void Init(UnitInfoContainer _container)
    {
        container = _container;
        arrTextInfo = GetComponentsInChildren<TextInfoBase>();

        foreach (TextInfoBase t in arrTextInfo)
            t.Init();
    }

    public void DisplayInfo()
    {
        arrTextInfo[0].UpdateText(container.maxHp);
        arrTextInfo[1].UpdateText(container.attDmg);
        arrTextInfo[2].UpdateText(container.attRange);
        arrTextInfo[3].UpdateText(container.attRate);
    }

    private UnitInfoContainer container = null;
    private TextInfoBase[] arrTextInfo = null;
}
