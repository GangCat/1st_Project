using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    private void Awake()
    {
        renderTexture = new RenderTexture(cameraRenderTexture);
        StartCoroutine("UpdateFogCoroutine");
    }

    private IEnumerator UpdateFogCoroutine()
    {
        while (true)
        {
            UpdateMaskMap();
            Camera.main.targetTexture = renderTexture;
            yield return null;
        }
    }

    private void UpdateMaskMap()
    {
        renderTexture = Camera.main.activeTexture;
        // 렌더 텍스쳐 새로 하나 제작
        // 마스크맵에서 유닛이 현재 있는 위치 주위는 alpha 1로 설정하기
        // 마스크맵에서 유닛이 있었던 위치는 alpha 0.5로 칠하기



        int idx = 0;
        while (idx < 1920 * 1080)
        {
            
            ++idx;
        }
        //renderTexture
    }





    [SerializeField]
    private Texture2D maskMap = null;
    [SerializeField]
    private Texture2D defaultMap = null;
    [SerializeField]
    private RenderTexture cameraRenderTexture = null;

    private RenderTexture renderTexture = null;
}
