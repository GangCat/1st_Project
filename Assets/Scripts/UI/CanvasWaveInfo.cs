using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWaveInfo : MonoBehaviour
{
    public void Init()
    {
        imageWaveProgressbar.Init();
    }

    public void Init(float _ttlBigWaveTime)
    {
        ttlBigWaveTime = _ttlBigWaveTime;
    }

    public void UpdateWaveTime(float _bigWaveTime_sec)
    {
        int min = (int)_bigWaveTime_sec / 60;
        int sec = (int)_bigWaveTime_sec % 60;
        textBigWaveTime.text = string.Format("{0}:{1}", min, sec);

        
        imageWaveProgressbar.UpdateLength((ttlBigWaveTime - _bigWaveTime_sec) / ttlBigWaveTime);
    }

    [SerializeField]
    private ImageProgressbar imageWaveProgressbar = null;
    [SerializeField]
    private TMP_Text textBigWaveTime = null;

    private float ttlBigWaveTime = 0f;
}
